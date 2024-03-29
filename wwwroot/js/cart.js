﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


const cartId = "standCart"
var official_cart;


// Se houver algum objeto de carrinho em sessão
if (sessionStorage.getItem(cartId) != null) {

    // get do objeto
    official_cart = JSON.parse(sessionStorage[cartId]);

    // se tiver o mesmo id do stand atual, já estamos com o carrinho certo
    if (official_cart.StandId == standCart.StandId) {
        console.log("not novo");
    }
    // se o id do stand for diferente, quer dizer que estamos com o carrinho de outro stand
    // e precisamos de reinicializar o carrinho
    else {
        console.log("novo");
        official_cart = standCart;
        sessionStorage.setItem("standCart", JSON.stringify(official_cart));
    }
}
// se não houver nenhum objeto de carrinho em sessão
else {
    console.log("novo");
    official_cart = standCart;
    sessionStorage.setItem("standCart", JSON.stringify(official_cart));
}

Object.keys(official_cart.Products).forEach(key => {
    if (official_cart.Products[key] === null) {
        delete official_cart.Products[key];
    }
});

// only do this after page load has been done
$(document).ready(function () {

    for (product in official_cart.Products) {

        if (product)
            updateValue(product, ((official_cart.Products)[product]).Quantity)
    }
})


//console.log(standCart);
console.log(official_cart);


function updateValue(productId, quantity) {

    if (productId)
        document.getElementById(productId).value = quantity;
}

function incrementValue(productId) {
    console.log(productId);
    if (productId) {
        var value = parseInt(document.getElementById(productId).value, 10);
        value = isNaN(value) ? 0 : value;
        value++;
        document.getElementById(productId).value = value;
    }
}

function decrementValue(productId) {

    if (productId) {

        var value = parseInt(document.getElementById(productId).value, 10);

        value = isNaN(value) ? 0 : value;

        if (value > 0) {

            --value;
            document.getElementById(productId).value = value;
    }
    }
}

function addProduct(name, productId, price, iva, stock) {
    console.log(name);
    if (productId in official_cart.Products) {

        // se já existir, o preço e a quantidade devem ser atualizados
        if (((official_cart.Products)[productId]).Quantity < stock) {

            ((official_cart.Products)[productId]).FinalPrice += price;
            ++((official_cart.Products)[productId]).Quantity;
            incrementValue(productId);
        }
    }
    else {

        // se nao existir, deve ser criada uma entrada para esse produto

        ((official_cart.Products)[productId]) = { Name: name, Id: productId, Quantity: 1, FinalPrice: price, IVA: iva }
        incrementValue(productId);
    }
    //console.log(official_cart);
    sessionStorage.setItem("standCart", JSON.stringify(official_cart));

}

function removeProduct(productId, price) {

    if (productId in official_cart.Products) {

        // se a quantidade for 1, a entrada deve ser removida

        if ( ((official_cart.Products)[productId]).Quantity == 1) {

            delete (official_cart.Products)[productId];
        }
        else {

            // se ainda existir, a quantidade deve ser reduzida

            ((official_cart.Products)[productId]).FinalPrice -= price;
            --((official_cart.Products)[productId]).Quantity;
        }
    }
    //console.log(official_cart);
    sessionStorage.setItem("standCart", JSON.stringify(official_cart));
    decrementValue(productId);

        // se nao existir, nada deve ser feito
}

function transplantCart() {

    var jsondata = JSON.stringify(official_cart);
    for (p in jsondata.Products) {
        if (p == null)
            delete  [jsondata.Products].p
    }
    console.log(jsondata);

    var answer;
    $.ajax({
        async: false,
        type: 'POST',
        url: '/Produtos/ReadJsonCart',
        contentType: "application/json; charset=utf-8",
        data: jsondata,
        cache: false,
        success: function (response) {
            answer = response;

        }
    });

    if (answer != null)
        document.getElementById("process_cart").click();
    else alert("Your cart is empty!");
    return answer;
}

function deleteCart() {

    sessionStorage.removeItem("standCart");
}
