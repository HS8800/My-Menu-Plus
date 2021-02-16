function scale() {



//Hide scroll controls when the screen can fit eveything
  setScrollPos(0);
  var scrollSpace = $('#select-main').width()-$('#select-scroll').width();
  if(scrollSpace > 0 && !$('#scroll-left')[0].classList.contains("hidden")){   
    $('.select-arrow').toggleClass("hidden");

  }else if( scrollSpace <= 0 && $('#scroll-left')[0].classList.contains("hidden")){
    $('.select-arrow').toggleClass("hidden");
  }

}

scale();
window.addEventListener("resize", scale);

$("#btn-show").click(function () {
  if ($("#main-description").height() == 70) {
    $("#main-description").animate({
      height: $("#main-description").get(0).scrollHeight,
    });
  } else {
    $("#main-description").animate({ height: "70px" });
  }
});


//Scroll to menu section
$("#select-scroll > div").click(function () {  
    $("#main-menu > h1:contains('"+ this.innerText+"')")[0].scrollIntoView({ behavior: "smooth" })
});

