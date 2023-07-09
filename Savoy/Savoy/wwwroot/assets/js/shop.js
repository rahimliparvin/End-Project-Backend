
//$(document).on("click", ".categoriesName", function () {

//    let categoryId = $(this).attr("data-id")
//    let changeElem = $(this)

//    let data = { id: categoryId }

//    $.ajax({
//        url: "Shop/GetCategoryProducts",
//        type: "Get",
//        data: data,
//        success: function (res) {
//            $(".content").html(res)

//        }
//    })
//})



//$(document).on("click", ".allCategoriesName", function (e) {
//    e.preventDefault();
//    e.stopPropagation();


//    $.ajax({
//        url: "Shop/GetAllCategoriesProducts",
//        type: "Get",
//        success: function (res) {
//            $(".content").html(res)

//        }
//    })
//})


//$(document).on("click", ".page", function () {


//    let categoryId = $(this).attr("data-id")
//    let changeElem = $(this)

//    let data = { id: categoryId }

//    $.ajax({
//        url: "Shop/GetCategoryProducts",
//        type: "Get",
//        data: data,
//        success: function (res) {
//            $(".content").html(res)

//        }
//    })

//})



"use strict";




///////////////////////////// Tab Menu ////////////////////

let headersTab = document.querySelectorAll("#categories-search-filter .categoriesname ul li");

let contentsTab = document.querySelectorAll("#categories-search-filter .content .item");

headersTab.forEach(header => {

    header.addEventListener("click", function () {

        document.querySelector("#categories-search-filter .categoriesname ul .active").classList.remove("active");
        header.classList.add("active");

        contentsTab.forEach(content => {
            if (content.getAttribute("data-id") == this.getAttribute("data-id")) {
                content.classList.remove("d-none");
            }
            else {
                content.classList.add("d-none");
            }
        });

    })

});


/////////////////////////////////



let firstImgss = document.querySelectorAll("#categories-search-filter .content .item .productcart .firstimg");

firstImgss.forEach(firstImg => {

    firstImg.addEventListener("mouseover", function () {
        this.classList.add("d-none");
    })

    let secondImgss = document.querySelectorAll("#categories-search-filter .content .item .productcart .secondimg");

    secondImgss.forEach(secondImg => {
        secondImg.addEventListener("mouseout", function () {
            firstImg.classList.remove("d-none");
        })

    });
});


//////////////// wishlist icon heart //////

let heartss = document.querySelectorAll(".content .productcart .like");

heartss.forEach(heart => {

    heart.addEventListener("click", function () {

        this.classList.add("d-none");
        this.nextElementSibling.classList.remove("d-none");
    })
});


let likeHeartss = document.querySelectorAll(".content .productcart .dislike");

likeHeartss.forEach(likeHeart => {

    likeHeart.addEventListener("click", function () {

        this.classList.add("d-none");
        this.previousElementSibling.classList.remove("d-none");
    })
});

/////////////////// Search button ///////////////

let search = document.querySelector("#categories-search-filter .filtersort ul :nth-child(2)");

search.addEventListener("click", function () {

   let searchArea = document.querySelector("#categories-search-filter .searcharea");

   if(searchArea.classList.contains("d-none")){

    searchArea.classList.remove("d-none");
 
    let filterArea = document.querySelector("#categories-search-filter .filter");

    filterArea.classList.add("d-none");

   }
   else{
    searchArea.classList.add("d-none");
    document.querySelector("#categories-search-filter .searcharea input").value = "";
   };

});


/////////////////// Close Search area /////////////////

let closeIcon = document.querySelector("#categories-search-filter .searcharea .fa-xmark");

closeIcon.addEventListener("click",function(){

    document.querySelector("#categories-search-filter .searcharea").classList.add("d-none");

    document.querySelector("#categories-search-filter .searcharea input").value = "";


});


///////////////// Filter area ////////////////////////////


let filter = document.querySelector("#categories-search-filter .filtersort ul :nth-child(1)");

filter.addEventListener("click",function(){

    let filterArea = document.querySelector("#categories-search-filter .filter");

    if(filterArea.classList.contains("d-none")){

        filterArea.classList.remove("d-none");

        document.querySelector("#categories-search-filter .searcharea").classList.add("d-none");

        document.querySelector("#categories-search-filter .searcharea input").value = "";
    }
    else{
        filterArea.classList.add("d-none");
    }
});


//////////////////////////// Carouse First Second Image //////////



let firstImages = document.querySelectorAll("#carousel .mySwiper .productcart .firstimg");


firstImages.forEach(firstImage => {

    firstImage.addEventListener("mouseover", function () {
        this.classList.add("d-none");
    })

    let secondImages = document.querySelectorAll("#carousel .mySwiper  .productcart .secondimg");

    secondImages.forEach(secondImage => {
        secondImage.addEventListener("mouseout", function () {
            firstImage.classList.remove("d-none");
        })

    });
});


////////////////////////////  Carousel Wishlist ///////////////////////////



let heartsCarousel = document.querySelectorAll(".mySwiper .productcart .like");

heartsCarousel.forEach(heart => {

    heart.addEventListener("click", function () {

        this.classList.add("d-none");
        this.nextElementSibling.classList.remove("d-none");
    })
});


let likeHeartsCarousel = document.querySelectorAll(".mySwiper .productcart .dislike");

likeHeartsCarousel.forEach(likeHeart => {

    likeHeart.addEventListener("click", function () {

        this.classList.add("d-none");
        this.previousElementSibling.classList.remove("d-none");
    })
});



