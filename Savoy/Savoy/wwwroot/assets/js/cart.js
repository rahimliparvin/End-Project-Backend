"use strict";


///////////////// Plus Minus /////////////////


let minus = document.querySelector("#cart .table .fa-minus");

minus.addEventListener("click",function(){

    let count = document.querySelector("#cart .table .item input").value;

   if(count > 1 ){

    count = count - 1;

    document.querySelector("#cart .table .item input").value = count;

   }
});

let plus = document.querySelector("#cart .table .fa-plus");

plus.addEventListener("click",function(){

    let count = document.querySelector("#cart .table .item input").value;

    count = parseInt(count) + 1;

    document.querySelector("#cart .table .item input").value = count;

});