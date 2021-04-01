function showKey(e) {
    $(e).find('.key').attr({ "type": "text" })  
}

function getParam(param) {
    var url = new URL(window.location.href)
    return url.searchParams.get(param);
}



$('#uploadBraintreeKeys').submit(function (e) {
    $("#loading-background").css({ "display": "block" })
    e.preventDefault();
    $.post('/Keys/updateBraintreeKeys', { content: getParam("content"), production: $("#chkEnviroment")[0].checked, MerchantID: $("#MerchantID").val(), PublicKey: $("#PublicKey").val(), PrivateKey: $("#PrivateKey").val(), __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val() }).done(function (response) {
        response = JSON.parse(response);

        if (response.response != null) {
            alert(response.response)

        } else {
            alert(response.error)

        }

    }).always(function () {
        $(".loading").css({ "display": "none" });
        $("#loading-background").css({ "display": "none" })
    });


}); 




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


$("#chkEnviroment").change(function () {
    if ($(this).is(':checked')) {
        $("#Environment").text("Production")
    } else {
        $("#Environment").text("Sandbox")
    }

});