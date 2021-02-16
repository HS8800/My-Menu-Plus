
var currentScrollDest = null;

function getScrollPos() {
  if ($("#select-scroll").css("transform") == "none") {
    return 0;
  }

  if(currentScrollDest == null){
    return $("#select-scroll")
    .css("transform")
    .split(",")[4]
    .trim()
    .replace("-", "");
  }else{
    var temp = currentScrollDest;
    currentScrollDest = null;
    return temp;
  }


}

function setScrollPos(x) {
  var scrollWidth = $("#select-scroll").width() - $("#select-main").width();

  if (x > scrollWidth) {//stop at end of scroll
    x = scrollWidth;
  }else if (x < 0) {//stop at begining of scroll
    x = 0;
  }
  currentScrollDest = x;//update scroll destination for animation

  $("#select-scroll").css({
    transform: "translateX(-" + x + "px)",
  });
}

const mainSelect = $('#select-main')[0];
let pos = { left: 0, x: 0 };


const mouseDownHandler = function (e) {
  $("#select-scroll").css({"transition-duration":"0s"});
  pos = {
    left: getScrollPos(),
    x: e.clientX,
  };

  document.addEventListener("mousemove", mouseMoveHandler);
  document.addEventListener("mouseup", mouseUpHandler);
};

const mouseMoveHandler = function (e) {
  const tempX = e.clientX - pos.x;
  vel = pos.left - tempX - getScrollPos();
  setScrollPos(pos.left - tempX);
};

const mouseUpHandler = function () {
  document.removeEventListener("mousemove", mouseMoveHandler);
  document.removeEventListener("mouseup", mouseUpHandler);
};

mainSelect.addEventListener("mousedown", mouseDownHandler);

$("#scroll-left").click(function () {
  $("#select-scroll").css({"transition-duration":"1s"});
  setScrollPos(
    Number(getScrollPos()) - mainSelect.scrollWidth / ($("#select-scroll").children().length - 2)
  );
});

$("#scroll-right").click(function () {
  $("#select-scroll").css({"transition-duration":"1s"});
  setScrollPos(
    Number(getScrollPos()) + mainSelect.scrollWidth / ($("#select-scroll").children().length - 2)
  );
});
