
function newNotification(text){
    $("#notification-container").append('<div class="notification"><span>'+text+'</span> <button onclick="this.parentNode.remove()">×</button></div>')  
    $($("#notification-container")[0].children[$("#notification-container")[0].children.length - 1]).fadeOut(0).fadeIn(1000).delay(6000).fadeOut(5000, function() {
        this.remove();
    });
}

function newError(text){
    $("#notification-container").append('<div class="notification error"><span>'+text+'</span> <button onclick="this.parentNode.remove()">×</button></div>')   
    $($("#notification-container")[0].children[$("#notification-container")[0].children.length - 1]).fadeOut(0).fadeIn(1000).delay(8000).fadeOut(5000, function() {
        this.remove();
    });
}

$(document).ready(function () {
    var loginResult = new URLSearchParams(window.location.search).get('login');

    if (loginResult != null) {
        if (loginResult == "success") {
            newNotification("Welcome")
        } else if (loginResult == "failed") {
            newError("Hmm, your credientails don't seem to be right, Please try again or use the Forgot Password button")
        }
    }
    
});



