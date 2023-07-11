"use strict";

/////////////

let shopBtn = document.querySelector(".shopBtn");


shopBtn.addEventListener("mouseover", function (e) {
    e.preventDefault();
    let shopPage = document.querySelector(".shop-pages");
    shopPage.classList.remove("d-none");
})

shopBtn.addEventListener("mouseout", function (e) {
    e.preventDefault();
    let shopPage = document.querySelector(".shop-pages");
    shopPage.classList.add("d-none");

    shopPage.addEventListener("mouseover", function () {
        shopPage.classList.remove("d-none");
    })

    shopPage.addEventListener("mouseout", function () {
        shopPage.classList.add("d-none");
    })
})


//////////

let categoriesBtn = document.querySelector(".categoriesBtn");

categoriesBtn.addEventListener("mouseover", function (e) {
    e.preventDefault();
    let categoriesTitle = document.querySelector(".categoriestitle");
    categoriesTitle.classList.remove("d-none");
})

categoriesBtn.addEventListener("mouseout", function (e) {
    e.preventDefault();
    let categoriesTitle = document.querySelector(".categoriestitle");
    categoriesTitle.classList.add("d-none");

    categoriesTitle.addEventListener("mouseover", function () {
        categoriesTitle.classList.remove("d-none");
    })

    categoriesTitle.addEventListener("mouseout", function () {
        categoriesTitle.classList.add("d-none");
    })
})

//////////////

let pagesBtn = document.querySelector(".pagesBtn");


pagesBtn.addEventListener("mouseover", function (e) {
    e.preventDefault();
    let pages = document.querySelector(".pages");
    pages.classList.remove("d-none");
})

pagesBtn.addEventListener("mouseout", function (e) {
    e.preventDefault();
    let pages = document.querySelector(".pages");
    pages.classList.add("d-none");

    pages.addEventListener("mouseover", function () {
        pages.classList.remove("d-none");
    })

    pages.addEventListener("mouseout", function () {
        pages.classList.add("d-none");
    })
})

///////////----Tab-Menu-----//////////

let headers = document.querySelectorAll("#tabmenu .tab-menu .item");

let contents = document.querySelectorAll("#tabmenu .content .item");

headers.forEach(element => {
    element.addEventListener("click", function () {
        document.querySelector("#tabmenu .tab-menu .active").classList.remove("active");
        this.classList.add("active");

        contents.forEach(content => {
            if (content.getAttribute("data-id") == this.getAttribute("data-id")) {
                content.classList.remove("d-none");
            }
            else {
                content.classList.add("d-none");
            }
        });

    })
});


let firstImgs = document.querySelectorAll("#tabmenu .content .item .productcart .firstimg");

firstImgs.forEach(firstImg => {

    firstImg.addEventListener("mouseover", function () {
        this.classList.add("d-none");
    })

    let secondImgs = document.querySelectorAll("#tabmenu .content .item .productcart .secondimg");

    secondImgs.forEach(secondImg => {
        secondImg.addEventListener("mouseout", function () {
            firstImg.classList.remove("d-none");
        })

    });
});


///////////////////



