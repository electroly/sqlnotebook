

function populate_toc () {
  var children = document.getElementsByTagName("h1").item(0).parentNode.childNodes
  var toc = ""

  var counters = new Array()
  counters[1] = 0
  counters[2] = 0
  counters[3] = 0
  counters[4] = 0

  /* Generate the table of contents */
  for(var ii=0; ii<children.length; ii++){
    var node = children.item(ii)
    var iHeader = -1
    if( node.tagName == "H1" ){ iHeader = 1 }
    if( node.tagName == "H2" ){ iHeader = 2 }
    if( node.tagName == "H3" ){ iHeader = 3 }
    if( node.tagName == "H4" ){ iHeader = 4 }

    if( iHeader>0 ){
      var anchor = "tocentry_" + ii

      for(var jj=iHeader+1; jj<=4; jj++){ counters[jj] = 0 }
      counters[iHeader]++

      var number = ""
      for(var jj=1; jj<=iHeader; jj++){ number += counters[jj] + "." }

      toc += '<div style="margin-left:' + (iHeader*6) + 'ex">'
      toc += '<a href="#' + anchor + '">' + number + " " + node.innerHTML
      toc += "</a></div>"
      
      var a = '<a style="color:inherit" name="' + anchor + '">' + number + '</a>'
      node.innerHTML = a + " " + node.innerHTML
    }
  }
  document.getElementById("toc").innerHTML = toc
}

function number_figs () {
  /* Number the figures in this document */
  var figcounter = 1
  var spans = document.getElementsByTagName("span")
  for(var ii=0; ii<spans.length; ii++){
    var s = spans.item(ii)
    if( s.className=="fig" ){
      s.innerHTML = figcounter
      figcounter++
    }
  }
}

function populate_refs () {
  /* Fix up <cite> references */
  var cites = document.getElementsByTagName("cite")
  for(var ii=0; ii<cites.length; ii++){
    var t = cites.item(ii).innerHTML
    var h = document.getElementById(t)

    if( !h ){
      alert("Bad reference: " + t)
      continue
    }

    var label
    if( h.tagName=="H1" || h.tagName=="H2"
     || h.tagName=="H3" || h.tagName=="H4"
    ){
      label = h.firstChild.firstChild.data
      label = label.substring(0, label.length-1)
    } else {
      label = h.firstChild.data
    }

    cites.item(ii).innerHTML = '<a href="#' + t + '">' + label + '</a>'
  }
}

function decorate_tables () {
  /* Decorate tables */
  var tables = document.getElementsByTagName("table")
  for(var ii=0; ii<tables.length; ii++){
    var t = tables.item(ii)
    if( t.className!="striped" ) continue
    var rows = t.rows
    for(var jj=1; jj<rows.length; jj += 2){
      rows.item(jj).style.backgroundColor = '#DDDDDD'
    }
  }
}

function check_for_duplicates () {
  var aReq = new Array();
  var ps = document.getElementsByTagName("p")

  for(var ii=0; ii<ps.length; ii++){
    var p = ps.item(ii)
    if( p.className!="req" || !p.id ) continue;

    if( aReq[p.id] ){
      alert("Duplicate requirement number: " + p.id)
    }
    aReq[p.id] = 1;
  }
}

onload = function () {
  number_figs()
  populate_toc()
  populate_refs()
  decorate_tables()
  check_for_duplicates()
}

