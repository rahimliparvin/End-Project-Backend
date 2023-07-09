"use strict";



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