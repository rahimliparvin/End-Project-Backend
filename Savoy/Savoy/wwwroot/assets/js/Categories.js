
$(document).on("click", ".categoriesName", function () {

    let categoryId = $(this).attr("data-id")
    let changeElem = $(this)

    let data = { id: categoryId }

    $.ajax({
        url: "Shop/GetCategoryProducts",
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
        url: "Shop/GetAllCategoriesProducts",
        type: "Get",
        success: function (res) {
            $(".content").html(res)

        }
    })
})
