function showKey(e) {
    $(e).find('.key').attr({ "type": "text" })  
}

function getParam(param) {
    var url = new URL(window.location.href)
    return url.searchParams.get(param);
}



$(".btn-key-new").click(function (e) {

    $("#key-table").hide();
    $(".loading").css({ "display": "block" });

    $.post('/Keys/GenerateKey', { content: getParam("content") , __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val() }).done(function (response) {
        response = JSON.parse(response);

        if (response.response != null) {

            if (e.target.dataset["reload"] == "true") {
                location.reload();
            }

            $("#key-table").show();
            $(".key").val(response.response);
        } else {
            alert(response.error)
            $("#key-table").show();
        }

    }).always(function () {
        $(".loading").css({ "display": "none" });
        
    });
})