"use strict";

//let shopBtn = document.querySelector(".shopBtn");


//shopBtn.addEventListener("mouseover",function(e){
//    e.preventDefault();
//    let shopPage = document.querySelector(".shop-pages");
//    shopPage.classList.remove("d-none");
//})

//shopBtn.addEventListener("mouseout",function(e){
//    e.preventDefault();
//    let shopPage = document.querySelector(".shop-pages");
//    shopPage.classList.add("d-none");

//    shopPage.addEventListener("mouseover", function(){
//        shopPage.classList.remove("d-none");
//    })

//    shopPage.addEventListener("mouseout", function(){
//        shopPage.classList.add("d-none");
//    })
//})



////////////////// Categories Header /////////////////////


//let categoriesBtn = document.querySelector(".categoriesBtn");

//categoriesBtn.addEventListener("mouseover",function(e){
//    e.preventDefault();
//    let categoriesTitle = document.querySelector(".categoriestitle");
//    categoriesTitle.classList.remove("d-none");
//})

//categoriesBtn.addEventListener("mouseout",function(e){
//    e.preventDefault();
//    let categoriesTitle = document.querySelector(".categoriestitle");
//    categoriesTitle.classList.add("d-none");

//    categoriesTitle.addEventListener("mouseover", function(){
//        categoriesTitle.classList.remove("d-none");
//    })

//    categoriesTitle.addEventListener("mouseout", function(){
//        categoriesTitle.classList.add("d-none");
//    })
//});

/////////////////////////


//let pagesBtn = document.querySelector(".pagesBtn");


//pagesBtn.addEventListener("mouseover",function(e){
//    e.preventDefault();
//    let pages = document.querySelector(".pages");
//    pages.classList.remove("d-none");
//})

//pagesBtn.addEventListener("mouseout",function(e){
//    e.preventDefault();
//    let pages = document.querySelector(".pages");
//    pages.classList.add("d-none");

//    pages.addEventListener("mouseover", function(){
//        pages.classList.remove("d-none");
//    })

//    pages.addEventListener("mouseout", function(){
//        pages.classList.add("d-none");
//    })
//});




///////////////// Tab-Menu /////////////////


//let headersBlog = document.querySelectorAll("#blogpage .tabmenu .item");

//let contentsBlog = document.querySelectorAll("#blogpage .content .item");

//headersBlog.forEach(element => {
//    element.addEventListener("click", function () {
//        document.querySelector("#blogpage .tabmenu .active").classList.remove("active");
//        this.classList.add("active");

//        contentsBlog.forEach(content => {
//            if (content.getAttribute("data-id") == this.getAttribute("data-id")) {
//                content.classList.remove("d-none");
//            }
//            else {
//                content.classList.add("d-none");
//            }
//        });

//    })
//});






///////////////// BlogPlus d-none ///////////////

let cartImgs = document.querySelectorAll("#blogpage .content .blogcart .cartimg");

cartImgs.forEach(cartImg => {
    
    cartImg.addEventListener("mouseover",function(){

        this.nextElementSibling.classList.remove("d-none");
    });

    cartImg.addEventListener("mouseout",function(){
        this.nextElementSibling.classList.add("d-none");
    });

    cartImg.nextElementSibling.addEventListener("mouseover",function(){

        this.classList.remove("d-none");
    });

    cartImg.nextElementSibling.addEventListener("mouseout",function(){

        this.classList.add("d-none");
    });

});


$(document).on("click", ".categoriesName", function () {

    let categoryId = $(this).attr("data-id")
    let changeElem = $(this)

    let data = { id: categoryId }

    $.ajax({
        url: "Blog/GetCategoryBlogs",
        type: "Get",
        data: data,
        success: function (res) {
            $(".content").html(res)

        }
    })
})



$(document).on("click", ".allCategoriesName", function (e) {
    e.preventDefault();
    e.stopPropagation();


    $.ajax({
        url: "Blog/GetAllCategoriesBlogs",
        type: "Get",
        success: function (res) {
            $(".content").html(res)

        }
    })
})