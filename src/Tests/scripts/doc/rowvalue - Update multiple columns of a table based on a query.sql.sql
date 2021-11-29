-- https://sqlite.org/rowvalue.html
CREATE TABLE ftsdocs (idxed, name, label, url, mtime, rid, type);
CREATE TABLE event (mtime, objid);
CREATE TABLE blob (uuid, rid);

UPDATE ftsdocs SET
  idxed=1,
  name=NULL,
  (label,url,mtime) = 
      (SELECT printf('Check-in [%%.16s] on %%s',blob.uuid,
                     datetime(event.mtime)),
              printf('/timeline?y=ci&c=%%.20s',blob.uuid),
              event.mtime
         FROM event, blob
        WHERE event.objid=ftsdocs.rid
          AND blob.rid=ftsdocs.rid)
WHERE ftsdocs.type='c' AND NOT ftsdocs.idxed

--output--
