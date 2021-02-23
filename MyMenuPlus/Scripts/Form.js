

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
        if (response.response == "success") {
            newNotification("Welcome")
        } else if (response.response == "failed") {
            newError(response.error)
        }     
    });
});


//Register
$("#register-form").submit(function (e) {
    //Stop Form Redirect
    e.preventDefault();

    //Button Feedback
    $(".btn-register").addClass("btn-state-sent")
    setTimeout(function () { $(".btn-state-sent").removeClass("btn-state-sent") }, 1000);
    //Send Post
    $.post('/Login/Register',
        {

            firstname: $("#registerFirstName").val(),
            secondname: $("#registerSecondName").val(),
            email: $("#registerEmail").val(),
            password: $("#registerPassword").val(),
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()

        }).done(function (response) {
            response = JSON.parse(response);

            if (response.response == "success") {
                newNotification("Welcome")
            } else if (response.response == "failed") {
                newError(response.error)
            } 
    });
});

//Passwords must match
$("#registerPasswordRepeat, #registerPassword").on('input', function () {
    if ($("#registerPassword").val() != $("#registerPasswordRepeat").val()) {
        $("#registerPassword")[0].setCustomValidity("Passwords must match.");
        $("#registerPassword")[0].checkValidity();
    } else {
        $("#registerPassword")[0].setCustomValidity();
        $("#registerPassword")[0].checkValidity();
    }
});




