document.addEventListener("DOMContentLoaded", function () {
    var modeSwitch = document.querySelector(".mode-switch");

    modeSwitch.addEventListener("click", function () {
        document.documentElement.classList.toggle("dark");
        modeSwitch.classList.toggle("active");
    });

    var listView = document.querySelector(".list-view");
    var gridView = document.querySelector(".grid-view");
    var projectsList = document.querySelector(".project-boxes");

    listView.addEventListener("click", function () {
        gridView.classList.remove("active");
        listView.classList.add("active");
        projectsList.classList.remove("jsGridView");
        projectsList.classList.add("jsListView");
    });

    gridView.addEventListener("click", function () {
        gridView.classList.add("active");
        listView.classList.remove("active");
        projectsList.classList.remove("jsListView");
        projectsList.classList.add("jsGridView");
    });

    document
        .querySelector(".messages-btn")
        .addEventListener("click", function () {
            document.querySelector(".messages-section").classList.add("show");
        });

    document
        .querySelector(".messages-close")
        .addEventListener("click", function () {
            document.querySelector(".messages-section").classList.remove("show");
        });

    document.querySelector(".add-btn").addEventListener("click", function () {
        document.querySelector(".messages-section").classList.remove("show");
    });

    document
        .querySelector(".messages-close")
        .addEventListener("click", function () {
            document.querySelector(".messages-section").classList.remove("show");
        });
});

const rfpButton = document.getElementById("rfp-button");
const formPopup = document.getElementById("form-popup");
const editFormPopup = document.getElementById("Edit-form-popup");
const editOverlay = document.getElementById("Edit-overlay");
const overlay = document.getElementById("overlay");
const closeButton = document.getElementById("close-button");
const editCloseButton = document.getElementById("edit-close-button");
function showModal() {
    formPopup.style.display = "block";
    overlay.style.display = "block";
}

function showForm(rfpId) {
    formPopup.style.display = "block";
    overlay.style.display = "block";
}

closeButton.addEventListener("click", () => {
    formPopup.style.display = "none";
    overlay.style.display = "none";
});

editCloseButton.addEventListener("click", () => {
    editFormPopup.style.display = "none";
    editOverlay.style.display = "none";
});


showInPopup = (url) => {
    editFormPopup.style.display = "block";
    editOverlay .style.display = "block";
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            console.log(res);
            $("#Edit-form-popup .modal-body").html(res);

        },
        error: function (xhr, status, error) {
            console.log("Error occurred while fetching data from URL: " + url);
            console.log("Error: " + error);
        }
    });
};

showInEditBidPopup = (url) => {
    console.log(url);
    editFormPopup.style.display = "block";
    editOverlay.style.display = "block";
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            console.log(res);
            $("#Edit-form-popup .modal-body").html(res);

        },
        error: function (xhr, status, error) {
            console.log("Error occurred while fetching data from URL: " + url);
            console.log("Error: " + error);
        }
    });
};

AddBidPopup = (url) => {
    console.log(url);
    editFormPopup.style.display = "block";
    editOverlay.style.display = "block";
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            console.log(res);
            $("#Edit-form-popup .modal-body").html(res);

        },
        error: function (xhr, status, error) {
            console.log("Error occurred while fetching data from URL: " + url);
            console.log("Error: " + error);
        }
    });
};







// When the user clicks on the project box, show the modal
// Get the popup
var popup = document.getElementById("myPopup");

// Get the <span> element that closes the popup
var span = document.getElementsByClassName("close")[0];

// When the user clicks on the project box, show the popup
function showPopup() {
    document.getElementById("myPopup").style.display = "block";
}

// When the user clicks on <span> (x), close the popup
function closePopup() {
    document.getElementById("myPopup").style.display = "none";
}

// When the user clicks anywhere outside of the popup, close it
window.onclick = function (event) {
    if (event.target == popup) {
        closePopup();
    }
};

// create rfp 
function loadSubcategories() {
    var categoryId = $("#categoryDropdown").val();
    if (categoryId) {
        $.ajax({
            url: "https://localhost:7014/api/SubCategories/ByCategoryId/" + categoryId,
            type: "GET",
            success: function (result) {
                console.log(result);
                $("#subcategoryDropdown").empty();
                $("#subcategoryDropdown").append(
                    $("<option>", { value: "", text: "Select a subcategory" })
                );
                $.each(result, function (i, subcategory) {
                    $("#subcategoryDropdown").append(
                        $("<option>", { value: subcategory.subCategoryId, text: subcategory.name })
                    );
                });
            },
        });
    }
}

function loadProducts() {
    var subcategoryId = $("#subcategoryDropdown").val();
    var categoryId = $("#categoryDropdown").val();
    if (subcategoryId) {
        $.ajax({
            url: "https://localhost:7014/api/ProductCatalogues/" + subcategoryId + "/" + categoryId,
            type: "GET",
            success: function (result) {
                console.log(result);
                $("#productDropdown").empty();
                $("#productDropdown").append(
                    $("<option>", { value: "", text: "Select a product" })
                );
                $.each(result, function (i, product) {
                    $("#productDropdown").append(
                        $("<option>", { value: product.id, text: product.name })
                    );
                });
            },
        });
    }
}
// edit rfp

function editLoadSubcategories() {
    var categoryId = $("#editCategoryDropdown").val();
    if (categoryId) {
        $.ajax({
            url: "https://localhost:7014/api/SubCategories/ByCategoryId/" + categoryId,
            type: "GET",
            success: function (result) {
                console.log(result);
                $("#editSubcategoryDropdown").empty();
                $("#editSubcategoryDropdown").append(
                    $("<option>", { value: "", text: "Select a subcategory" })
                );
                $.each(result, function (i, subcategory) {
                    $("#editSubcategoryDropdown").append(
                        $("<option>", { value: subcategory.subCategoryId, text: subcategory.name })
                    );
                });
            },
        });
    }
}

function editLoadProducts() {
    var subcategoryId = $("#editSubcategoryDropdown").val();
    var categoryId = $("#editCategoryDropdown").val();
    if (subcategoryId) {
        $.ajax({
            url: "https://localhost:7014/api/ProductCatalogues/" + subcategoryId + "/" + categoryId,
            type: "GET",
            success: function (result) {
                console.log(result);
                $("#editProductDropdown").empty();
                $("#editProductDropdown").append(
                    $("<option>", { value: "", text: "Select a product" })
                );
                $.each(result, function (i, product) {
                    $("#editProductDropdown").append(
                        $("<option>", { value: product.id, text: product.name })
                    );
                });
            },
        });
    }
}