

//Login Menu Open & Close Controls
$(".login-close").click(function () {
    $("#login-popup").css({ "display": "none" });

    $("#register-form").css({ "display": "none" });
    $("#login-form").css({ "display": "block" });
    $(".errorDisplay").text("");
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
        $(".errorDisplay").text("");
    } else {
        $("#register-form").css({ "display": "none" });
        $("#login-form").css({ "display": "block" });
        $(".errorDisplay").text("");
    }
});


//Login
$("#login-form").submit(function (e) {
    //Stop Form Redirect
    e.preventDefault();

    //Button Feedback
    $(".btn-login").addClass("btn-state-sent")
    $(".loading").css({ "display": "block" })
    setTimeout(function () { $(".btn-state-sent").removeClass("btn-state-sent"); $(".loading").css({ "display": "none" }) }, 10000);

    //Send Post
    $.post('/Login/Login', { email: $("#login-email").val(), password: $("#login-password").val(), __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()}).done(function (response) {
        response = JSON.parse(response);
        if (response.response == "success") {
            window.location.href = "/MenuSelection";//Redirect to menu selection
            newNotification("Welcome")
            $(".errorDisplay").text("");           
        } else if (response.response == "failed") {
            $(".errorDisplay").text(response.error)
        }     
    }).always(function () {
        $(".btn-state-sent").removeClass("btn-state-sent")
        $(".loading").css({ "display": "none" })
    });
    password: $("#login-password").val("");
    
});


//Register
$("#register-form").submit(function (e) {
    //Stop Form Redirect
    e.preventDefault();

    //Button Feedback
    $(".btn-register").addClass("btn-state-sent")
    $(".loading").css({ "display": "block" })
    setTimeout(function () { $(".btn-state-sent").removeClass("btn-state-sent"); $(".loading").css({ "display": "none" }) }, 10000);
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
                newNotification("Welcome");
         
                //go to login form
                $("#register-form").css({ "display": "none" });
                $("#login-form").css({ "display": "block" });
                $(".errorDisplay").text("");


            } else if (response.response == "failed") {
                $(".errorDisplay").text(response.error)
            } 
        }).always(function () {
            $(".btn-state-sent").removeClass("btn-state-sent")
            $(".loading").css({ "display": "none" })
        });
});

//Passwords must match
$("#registerPasswordRepeat, #registerPassword").on('input', function () {
    if ($("#registerPassword").val() != $("#registerPasswordRepeat").val()) {
        $("#registerPassword")[0].setCustomValidity("Passwords must match.");
        $("#registerPassword")[0].checkValidity();
    } else {
        $("#registerPassword")[0].setCustomValidity("");
        $("#registerPassword")[0].checkValidity();
    }
});


function getCookie(name) {
    var dc = document.cookie;
    var prefix = name + "=";
    var begin = dc.indexOf("; " + prefix);
    if (begin == -1) {
        begin = dc.indexOf(prefix);
        if (begin != 0) return null;
    }
    else {
        begin += 2;
        var end = document.cookie.indexOf(";", begin);
        if (end == -1) {
            end = dc.length;
        }
    }
  
    return decodeURI(dc.substring(begin + prefix.length, end));
} 




$(".getStarted").click(function () {
    $("#login-popup").css({ "display": "block" });

    $("#register-form").css({ "display": "block" });
    $("#login-form").css({ "display": "none" });
    $(".errorDisplay").text("");
});
