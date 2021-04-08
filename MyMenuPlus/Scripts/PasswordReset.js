

$("#new-password, #new-password-repeat").on('input', function () {
    if ($("#new-password").val() != $("#new-password-repeat").val()) {
        $("#new-password")[0].setCustomValidity("Passwords must match.");
        $("#new-password")[0].checkValidity();
    } else {
        $("#new-password")[0].setCustomValidity("");
        $("#new-password")[0].checkValidity();
    }
});

$("#reset-password-form").submit(function (e) {
    e.preventDefault();
    $("#loading-background").css({ "display": "flex" })
    $.post('/Login/NewPassword', { email: $("#email").val(), code: $("#code").val(), password: $("#new-password").val(), __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val() }).done(function (response) {
        response = JSON.parse(response);
        if (response.response == "success") {
            alert("success");
            window.location.href = "/";
        } else if (response.response == "failed") {
            $(".errorDisplay").text(response.error)
        }
    }).always(function () {
        $("#loading-background").css({ "display": "none" })
    });
}); 