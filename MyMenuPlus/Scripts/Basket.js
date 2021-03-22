﻿var basket = []


function basketAdd(id,name,qty,price) {
    var itemExists = basket.filter(e => e.id == id);
    if (itemExists.length > 0) {

        if (itemExists[0].name != name) {
            alert("Basket Error: Items in basket don't match current items")
        } else {
            itemExists[0].qty = Number(itemExists[0].qty) + Number(qty);
        }
     
    } else {
        basket.push({ id: id, name: name, qty: qty, price: price})
    } 

    refreshBasketTable()
}

function createSeletion(selected) {
    var selectionHTML = "<select onchange='updateBasket(this)'>";
    selectionHTML += "<option value='0'>Remove</option>";
    for (let i = 1; i < 100; i++) {
        if (selected == i) {
            selectionHTML += "<option value='" + i + "' Selected>"+i+"</option>";
        } else {
            selectionHTML += "<option value='" + i + "'>" + i +"</option>";
        }   
    }
    selectionHTML += "</select>";
    return selectionHTML;
}

function updateBasket(selection) {
    if (selection.value == 0) {
        basket.splice(basket.findIndex((e => e.id == selection.parentNode.parentNode.dataset.id)), 1);     
    } else {
        var itemExists = basket.filter(e => e.id == selection.parentNode.parentNode.dataset.id);
        itemExists[0].qty = selection.value;
    }

    refreshBasketTable();
}

function refreshBasketTable() {
    if (basket.length != 0) {
        $("#basket-empty").hide();

        
        $("#basket-table").show();
        $("#basket-table").empty();
        $("#basket-total").show();

        $("#basket-table").append("<thead id='basket-thead'></thead>")

        $("#basket-thead").append(`
            <tr>
                <th>Item</th>
                <th>Qty</th>
                <th>Total</th>
            </tr>`);

        var total = 0;


        $("#basket-table").append("<tbody id='basket-content'></tbody>")

        for (let i = 0; i < basket.length; i++) {

            var totalOfItem = Number(basket[i].price.replace(/£/g, '')) * Number(basket[i].qty);
            total += totalOfItem;

            $("#basket-content").append(`
                <tr data-id=`+basket[i].id+`>
                    <td>` + basket[i].name + `</td>
                    <td>` + createSeletion(basket[i].qty) + `</td>
                    <td>` + currency.format(totalOfItem) + `</td>
                </tr>`);
        }

      

        $("#basket-total-value").text(currency.format(total));

    } else {
        $("#basket-total").hide();
        $("#basket-table").empty();
        $("#basket-table").hide();
        $("#basket-empty").show();
    }
}

$(".item-add").click(function () {    
    var item = this.parentNode.parentNode;
    basketAdd(item.dataset.itemid, $(item).find(".item-name").text(), 1, $(item).find(".item-price").text())
});

$(".basket-btn").click(function () {

    if ($("#shop-more-container").css("display") != "flex") {//First step confirm order
        $("#main-menu").hide();
        $("#basket-container").css({ "max-width": "initial" });
        $("#shop-more-container").css({ "display": "flex" });
        
    } else {//Second step collect payment
        $("#payment-form").css({ "display": "block" });   
        $("#basket-list").css({ "display": "none" });  
        $(".basket-btn").css({ "display": "none" });  
        $("#btn-shop-back").css({ "display": "block" });   
    }

});

$("#btn-shop-more").click(function () {
    $("#main-menu").show();
    $("#basket-container").css({ "max-width": "440px" });
    $("#shop-more-container").css({ "display": "none" });
    $("#payment-form").css({ "display": "none" });  
});

$("#btn-shop-back").click(function () {
    $("#payment-form").css({ "display": "none" });  
    $("#btn-shop-back").css({ "display": "none" });   
    $("#basket-list").css({ "display": "block" });  
    $("#shop-more-container").css({ "display": "flex" });
    $(".basket-btn").css({ "display": "block" });  

});
