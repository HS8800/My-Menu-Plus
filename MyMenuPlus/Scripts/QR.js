
function getParam(param) {
	var url = new URL(window.location.href)
	return url.searchParams.get(param);
}


var qrID = 1;

function generateQRCode(text) {

	$("#qr-container").append(`
		<div class="qr">
			<div id="qr-`+qrID+`"></div>
			<div class="qr-text">`+text+`</div>
		</div>
	`);

	

	var canvas = document.createElement('canvas');
	var qr = new QRious({
		element: canvas,
		size: 200,
		value: "www.mymenuplus.com/Menu?content=" + getParam("content")+"&table=" + text
	});
	
	
	$("#qr-" + qrID).append(canvas);

	qrID++;
}

$("#QR-generate").click(function () {
	generateQRCode($("#input-table-number").val());
	$("#input-table-number").val("");
});