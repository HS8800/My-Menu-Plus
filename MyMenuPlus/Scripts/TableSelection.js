

$("#table-background,#btn-table-skip").click(function (e) {
    if (e.target !== this)
        return;

    $("#table-background").css({ "display": "none" });
});

//function from https://stackoverflow.com/questions/486896/adding-a-parameter-to-the-url-with-javascript
function insertParam(key, value) {
    key = encodeURIComponent(key);
    value = encodeURIComponent(value);
 
    var kvp = document.location.search.substr(1).split('&');
    let i = 0;

    for (; i < kvp.length; i++) {
        if (kvp[i].startsWith(key + '=')) {
            let pair = kvp[i].split('=');
            pair[1] = value;
            kvp[i] = pair.join('=');
            break;
        }
    }

    if (i >= kvp.length) {
        kvp[kvp.length] = [key, value].join('=');
    }
    let params = kvp.join('&');

    document.location.search = params;
}

function getParam(param) {
    var url = new URL(window.location.href)
    return url.searchParams.get(param);
}

//Set table number from url as varable to prevent having to reload the page if the user gives a table number later
var table = -1;
if (getParam("table") != null) {
    table = getParam("table");
}

$("#table-container").submit(function (e) {  
    e.preventDefault();
    insertParam("table", $("#input-table-number").val());
});

$(document).ready(function () {
    var table = getParam("table");

    if (table == null && $("#notOpen-background").css("display") != "block") {
        $("#table-background").css({ "display": "block" });
    } else {
        $("#table-background").css({ "display": "none" });
    }


});


$("#basket-table-update").click(function () {
    $("#table-background").show();
    $("#btn-table-qr").hide();
    $("#table-container").unbind();
    $("#table-container").submit(function (e) {
        e.preventDefault();
        table = $("#input-table-number").val();
        $("#table-background").hide();
        $("#basket-table-number-value").text(table);
    });
});