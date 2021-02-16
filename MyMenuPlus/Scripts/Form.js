

//Login Menu Open & Close Controls
$(".login-close").click(function () {
    $("#login-popup").css({ "display": "none" });
});

$("#login-show").click(function () {
    $("#login-popup").css({ "display": "block" });
});


//Login Menu Switch Between Login And New Member Forms
$(".form-message").click(function () {
    //Reset Form Submit Button Feedback
    $(".btn-state-sent").removeClass("btn-state-sent") 

    //Toggle Forms
    if ($("#login-form").css("display") == "block") {
        $("#register-form").css({ "display": "block" });
        $("#login-form").css({ "display": "none" });
    } else {
        $("#register-form").css({ "display": "none" });
        $("#login-form").css({ "display": "block" });
    }
});


//Login
$("#login-form").submit(function (e) {
    //Stop Form Redirect
    e.preventDefault();

    //Button Feedback
    $(".btn-login").addClass("btn-state-sent")
    setTimeout(function () { $(".btn-state-sent").removeClass("btn-state-sent") }, 1000);

    //Send Post
    $.post('/Login/Login', { email: $("#login-email").val(), password: $("#login-password").val(), __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()}).done(function (response) {
        response = JSON.parse(response);

        if (response.response == "Success") {
            newNotification("Welcome")
        } else if (response.response == "Failed") {
            newError("Hmm, your credientails don't seem to be right, Please try again or use the Forgot Password button")
        } else if (response.error != null) {
            newError(response.error)
        } else {
            newError("Something unexpected occurred")
        }

        
    });
});




