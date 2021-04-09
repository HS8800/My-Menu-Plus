
function getItemPropertys(itemId) {
    var item = $("#main-menu").find("tr[data-itemid=" + itemId + "]")[0];

    var prop = {};

    prop.isVeg = false;
    if ($(item).find(".fa-apple-alt").length != 0) {
        prop.isVeg = true;
    }

    prop.isSpicy = false;
    if ($(item).find(".fa-pepper-hot").length != 0) {
        prop.isSpicy = true;
    }

    prop.isSnack = false;
    if ($(item).find(".fa-cookie-bite").length != 0) {
        prop.isSnack = true;
    }

    prop.isDrink = false;
    if ($(item).find(".fa-coffee").length != 0) {
        prop.isDrink = true;
    }

    return prop
}


function percent(value,total) {
    return ((100 / total) * value) / 100;
}

function recommendationBuild() {

    var itemTypes = { mains: 0, snacks: 0, drinks: 0 }
    var extra = { isVeg: 0, isSpicy: 0}

    //order by basket item propertys
    for (let i = 0; i < basket.length; i++) {
        var itemProp = getItemPropertys(basket[i].id);

        if (itemProp.isSpicy) {
            extra.isSpicy += Number(basket[i].qty);
        }

        if (itemProp.isVeg) {
            extra.isVeg += Number(basket[i].qty);
        }

        if (itemProp.isDrink) {
            itemTypes.drinks += Number(basket[i].qty);
        } else if (itemProp.isSnack) {
            itemTypes.snacks += Number(basket[i].qty);
        } else {
            itemTypes.mains += Number(basket[i].qty);
        }
    }


    //total non drinks
    var nonDrinksTotal = itemTypes.snacks + itemTypes.mains;

    //find largest type
    var largest = itemTypes[Object.keys(itemTypes).reduce(function (a, b) { return itemTypes[a] > itemTypes[b] ? a : b })]

    //find missing items for complete sets
    itemArray = Object.entries(itemTypes);
    var totalForCompleteSets = 0;

    for (let i = 0; i < itemArray.length; i++) {
        itemArray[i][1] -= largest;
        itemArray[i][1] = Math.abs(itemArray[i][1]);
        totalForCompleteSets += itemArray[i][1];
    }


    //build nice recommendation object
    var recommendationScores = {}
    recommendationScores.mains = percent(itemArray[0][1], totalForCompleteSets);
    recommendationScores.snacks = percent(itemArray[1][1], totalForCompleteSets);
    recommendationScores.drinks = percent(itemArray[2][1], totalForCompleteSets);
    recommendationScores.spicyFoodTendency = percent(extra.isSpicy, nonDrinksTotal);
    recommendationScores.vegFoodTendency = percent(extra.isVeg, nonDrinksTotal);


    var itemScores = [];
    var itemsWithImages = $("tr").find(".menu-item-img").parent().parent();
    itemsWithImages.each(function () {
        var itemProp = getItemPropertys(this.dataset.itemid);
        var itemScore = { id: this.dataset.itemid, score: 0}
     
        
        if (itemProp.isDrink) {//drink           
            itemScore.score += recommendationScores.drinks;
        } else {

            if (itemProp.isSnack) {//snack
                itemScore.score += recommendationScores.snacks;
            } else {//main
                itemScore.score += recommendationScores.mains;
            }

          
            if (itemProp.isSpicy) {
                itemScore.score += recommendationScores.spicyFoodTendency;
            }  
          

            if (itemProp.isVeg) {
                itemScore.score += recommendationScores.vegFoodTendency;
            }
        }

       
        itemScores.push(itemScore);
        
    });

    itemScores = itemScores.sort((a, b) => parseFloat(a.score) - parseFloat(b.score)).reverse();
    console.log(itemScores)

    try { populateRecommendation(itemScores[0].id) } catch (e) { }
    try { populateRecommendation(itemScores[1].id) } catch (e) { }
    try { populateRecommendation(itemScores[2].id) } catch (e) { }
}

var recommendationCount = 0;
function populateRecommendation(id) {

    if (recommendationCount > 2) {
        recommendationCount = 0;
    }


    var item = $("tr[data-itemid=" + id + "]")[0];
    var name = $(item).find(".item-name").text();
    var price = $(item).find(".item-price").text()
    var image = $(item).find(".menu-item-img").css("background-image");

    var rec = $(".rec > div");

    $(rec[recommendationCount])[0].dataset.itemid = id;
    $(rec[recommendationCount]).css({"display":"flex"});
    $(rec[recommendationCount]).find(".rec-item-name").text(name);
    $(rec[recommendationCount]).find(".item-price").text(price);
    $(rec[recommendationCount]).find(".rec-image").css({ "background-image": image });
    recommendationCount++

    

}

//recommendationBuild();



