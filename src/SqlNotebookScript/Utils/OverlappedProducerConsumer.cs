using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace SqlNotebookScript.Utils {
    public static class OverlappedProducerConsumer {
        public delegate void ProducerDelegate<TBuffer>(TBuffer buffer, out int batchCount);
        public delegate void ConsumerDelegate<TBuffer>(TBuffer buffer, int batchCount, long totalSoFar);

        public static void Go<TBuffer>(
            Func<TBuffer> newBufferFunc,
            ProducerDelegate<TBuffer> producer,
            ConsumerDelegate<TBuffer> consumer,
            CancellationToken cancel
            ) {
            BlockingCollection<TBuffer> unusedBuffers = new();
            unusedBuffers.Add(newBufferFunc(), cancel);
            unusedBuffers.Add(newBufferFunc(), cancel);

            BlockingCollection<(int Count, TBuffer Buffer)> consumerQueue = new();
            
            long total = 0;

            var producerTask = Task.Factory.StartNew(
                () => {
                    while (!cancel.IsCancellationRequested) {
                        var buffer = unusedBuffers.Take(cancel);
                        producer(buffer, out var batchCount);
                        if (batchCount <= 0) {
                            break;
                        }
                        consumerQueue.Add((batchCount, buffer));
                    }
                    consumerQueue.CompleteAdding();
                }, cancel, TaskCreationOptions.LongRunning, TaskScheduler.Default);

            var consumerTask = Task.Factory.StartNew(
                () => {
                    try {
                        while (!cancel.IsCancellationRequested) {
                            var (count, buffer) = consumerQueue.Take(cancel);
                            total += count;
                            consumer(buffer, count, total);
                            unusedBuffers.Add(buffer);
                        }
                    } catch (InvalidOperationException) {
                        // thrown when we try to Take after CompleteAdding; just end the loop
                    }
                }, cancel, TaskCreationOptions.LongRunning, TaskScheduler.Default);

            producerTask.ConfigureAwait(false).GetAwaiter().GetResult();
            consumerTask.ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
