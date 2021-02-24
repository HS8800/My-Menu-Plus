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


//Scroll to menu section
$("#select-scroll > div").click(function () {  
    $("#main-menu > h1:contains('"+ this.innerText+"')")[0].scrollIntoView({ behavior: "smooth" })
});

