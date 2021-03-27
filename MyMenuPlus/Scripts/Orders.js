$(':root').css('--order-width','300px');


var columns = Math.floor($('body').css('width').slice(0, -2)/($(':root').css('--order-width').slice(0, -2)));
for(let i = 0;i<columns;i++){
    $("#breakers").append("<div class='breaker'></div>")
}  
breakerEvents()




var dragging = false;

function breakerEvents(){
    $(".breaker").mousedown(function() {
        console.log("down")
        if(!dragging){
            dragging = true;
            $("body").mousemove(function(e) {
                $(':root').css('--order-width',(e.pageX-20)+'px');
                
                var columns = Math.floor($('body').css('width').slice(0, -2)/(e.pageX-20));
                
                if(columns != $(".breaker").length){
                    $("#breakers").empty();
                    for(let i = 0;i<columns;i++){
                        $("#breakers").append("<div class='breaker'></div>")
                    }
                    breakerEvents()
                }

            });
            
        }
    });


    $("body").mouseup(function() {
        if(dragging){
            console.log("up")
            $( "body").unbind( "mousemove" );
            dragging = false;
        }
    });
}



function timeConditionColor(minutes,color,timer){
    if((Number(timer[1].textContent) + Number(timer[0].textContent)*60) > minutes){
        let order = timer[0].parentNode.parentNode;
        if($(order).css("border-color") != color){
            $(order).css({"border-color":color})
            $(order).find(".timer").css({"border-color":color})
            $(order).find("tbody > tr:nth-child(1) > td").css({"border-color":color})
        }
    }
}

setInterval(function(){ 
    var timers = $(".timer")    
    for(let i = 0;i<timers.length;i++){
        let time = $(timers[i]).find("span");
        if(time[2].textContent == "59"){
            time[2].textContent = 0;
            if(time[1].textContent == "59"){
                time[1].textContent = 0;           
                time[0].textContent = Number(time[0].textContent)+1
            }else{
                time[1].textContent = Number(time[1].textContent)+1
            }
        }else{
            time[2].textContent = Number(time[2].textContent)+1
        }        
        timeConditionColor(10,"yellow",time);
        timeConditionColor(25,"orange",time);
        timeConditionColor(30,"red",time);
    }

   

}, 1000);

function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}

function newOrder(id,table,items) {

    id = htmlEncode(id);
    table = htmlEncode(table);
    items = htmlEncode(items);

    var order = `
        <div>
            <div class="timer"><span>0</span>:<span>0</span>:<span>0</span></div>
            <div class="order-content">
                <h2>Order <span>`+id+`</span></h2>
                <h4 class="table-number">Table <span>`+table+`</span></h4>
            </div>
            <table class="item-table">
                <tbody>
                    <tr>
                        <td>2</td>
                        <td>Burger</td>
                    </tr>
                    <tr>
                        <td>2</td>
                        <td>Burger</td>
                    </tr>
                </tbody>
            </table>
            <button class="order-complete">Complete</button>
        </div>
    `
    $("#order-container").append(order);




}
