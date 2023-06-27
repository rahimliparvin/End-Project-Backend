"use strict"



let headersTab = document.querySelectorAll("#carouselshop .productdetail .headers li");

headersTab.forEach(header => {

  header.addEventListener("click", function () {

    let as = document.querySelector("#carouselshop .productdetail .headers .active");

    as.classList.remove("active");

    header.classList.add("active");

    let contents = document.querySelectorAll("#carouselshop .productdetail .content div");

    contents.forEach(content => {

      if (content.getAttribute("data-id") == header.getAttribute("data-id")) {

        content.classList.remove("d-none");
      }
      else {
        content.classList.add("d-none");
      }

    });


  })

});

let minus = document.querySelector(".productinfos .quantity .fa-minus");

minus.addEventListener("click", function () {

  let num = this.parentNode.nextElementSibling.firstElementChild.value;

  if (num > 1) {

    num = num - 1;
    this.parentNode.nextElementSibling.firstElementChild.value = num;
  }
  else {

    return;

  }

});

let plus = document.querySelector(".productinfos .quantity .fa-plus");

plus.addEventListener("click", function () {

  let num = this.parentNode.previousElementSibling.firstElementChild.value;

  let newNum = parseInt(num) + 1;

  this.parentNode.previousElementSibling.firstElementChild.value = newNum;


})

