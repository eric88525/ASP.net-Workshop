var bookDataFromLocalStorage = [];

$(document).ready(function () {

    loadBookData();
    var data = [{
            text: "資料庫",
            value: "database"
        },
        {
            text: "網際網路",
            value: "internet"
        },
        {
            text: "應用系統整合",
            value: "system"
        },
        {
            text: "家庭保健",
            value: "home"
        },
        {
            text: "語言",
            value: "language"
        }
    ]

    // pick the book date
    $("#bought_datepicker").kendoDatePicker({
        format: "yyyy/MM/dd"
    });

    // selete book category and change img
    $("#book_category").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: data,
        index: 0,
        change: onChange
    });

    // debug
    $("#show_button").kendoButton({
        click: showButtonOnclick
    });

    // vakidator
    var validator = $("#myform").kendoValidator().data("kendoValidator");

    // Validate the input when the Save button is clicked
    $("#add_button").on("click", function () {
        if (validator.validate()) {
            // If the form is valid, the Validator will return true
            addButtonOnclick();

        }
    });

    $("#book_grid").kendoGrid({
        dataSource: {
            data: bookDataFromLocalStorage,
            schema: {
                model: {
                    fields: {
                        BookId: {
                            type: "int"
                        },
                        BookName: {
                            type: "string"
                        },
                        BookCategory: {
                            type: "string"
                        },
                        BookAuthor: {
                            type: "string"
                        },
                        BookBoughtDate: {
                            type: "string"
                        }
                    }
                }
            },
            pageSize: 20,
        },
        toolbar: kendo.template("<div  class='book-grid-toolbar'><input id='book-filter' class='book-grid-search' placeholder='我想要找......' type='text'></input></div>"),
        height: 550,
        editable: true,
        sortable: true,
        editable: true,
        editable: "inline",
        filterable: true,
        pageable: true,
        batch: true,
        pageable: {
            input: true,
            numeric: false
        },
        columns: [{
                field: "BookId",
                title: "書籍編號",
                width: "10%"
            },
            {
                field: "BookName",
                title: "書籍名稱",
                width: "50%",
                filterable: {
                    cell: {
                        operator: "contains",
                        suggestionOperator: "contains"
                    }
                }
            },
            {
                field: "BookCategory",
                title: "書籍種類",
                width: "10%"
            },
            {
                field: "BookAuthor",
                title: "作者",
                width: "15%"
            },
            {
                field: "BookBoughtDate",
                title: "購買日期",
                width: "15%"
            },
            {
                command: {

                    name: "details",
                    text: "delete",
                    click: function (e) {
                        // prevent page scroll position change
                        e.preventDefault();
                        // e.target is the DOM element representing the button
                        var tr = $(e.target).closest("tr"); // get the current table row (tr)
                        // get the data bound to the current table row
                        var data = this.dataItem(tr);
                        deleteBook(data.BookId);
                    }
                },
                title: " ",
                width: "120px"
            }
        ]

    });

    // get the search condition and filter
    $('#book-filter').on("input", function () {
        console.log('change');
        var grid = $('#book_grid').data('kendoGrid');
        var field = 'BookName';
        var operator = 'contains';
        var value = this.value;
        grid.dataSource.filter({
            field: field,
            operator: operator,
            value: value
        });
    });
    ///

    // click to open  window
    var myWindow = $("#window"),
        // button & window
        undo = $("#window_button"); 

    undo.click(function () {
        // open window
        myWindow.data("kendoWindow").open();
        undo.fadeOut();
    });

    // set the window property
    myWindow.kendoWindow({
        width: '500px',
        height: 'auto',
        title: "Add new book",
        visible: false,
        actions: [
            "Pin",
            "Minimize",
            "Maximize",
            "Close"
        ],
        close: onClose
    }).data("kendoWindow").center();

    function onClose() {
        undo.fadeIn();
    }
})


// add the book
function addButtonOnclick() {
    var id = bookDataFromLocalStorage[bookDataFromLocalStorage.length - 1].BookId;
    var bookCategory = $("#book_category").data("kendoDropDownList").text();
    var bookName = $("#book_name").val();

    // check if the book name  exist or not
    for (let i = 0; i < bookDataFromLocalStorage.length; i++) {
        if (bookDataFromLocalStorage[i].BookName == String(bookName)) {
            window.alert("book name duplicate!");
            return;
        }
    }

    // fill data value and add in bookDataFromLocalStorage
    var bookAuthor = $("#book_author").val();
    var date = kendo.toString($("#bought_datepicker").data("kendoDatePicker").value(), "yyyy-MM-dd");
    var pub = "I dont know ok?"
    var addData = {
        "BookId": Number(id + 1),
        "BookCategory": String(bookCategory),
        "BookName": String(bookName),
        "BookAuthor": String(bookAuthor),
        "BookBoughtDate": String(date),
        "BookPublisher": String(pub)
    }
    bookDataFromLocalStorage.push(addData);
    localStorage.setItem("bookData", JSON.stringify(bookDataFromLocalStorage));
    refreshGrid();
}

// Get book data from local and save into bookDataFromLocalStorage
function loadBookData() {
    bookDataFromLocalStorage = JSON.parse(localStorage.getItem("bookData"));
    if (bookDataFromLocalStorage == null) {
        bookDataFromLocalStorage = bookData;
        localStorage.setItem("bookData", JSON.stringify(bookDataFromLocalStorage));
    }
}

// when dropdownlist change,do this
function onChange() {
    var data = $("#book_category").data("kendoDropDownList");
    $(".book-image").attr("src", "image/" + data.value() + ".jpg");
    console.log(data.text());
}

// delete book from bookDataFromLocalStorage
function deleteBook(e) {
    var book_idx = bookDataFromLocalStorage.findIndex(function (book) {
        return book.BookId === e
    });
    bookDataFromLocalStorage.splice(book_idx, 1);
    localStorage.setItem("bookData", JSON.stringify(bookDataFromLocalStorage));
    console.log(bookDataFromLocalStorage);
    refreshGrid();
}

// refresh grid
function refreshGrid() {
    var grid = $("#book_grid").data('kendoGrid');
    grid.dataSource.read();
    grid.refresh();
}

// Use to debug,show some information
function showButtonOnclick() {
    var id = bookDataFromLocalStorage.length
    var bookCategory = $("#book_category").data("kendoDropDownList").text();
    var bookName = $("#book_name").val();
    var bookAuthor = $("#book_author").val();
    var date = kendo.toString($("#bought_datepicker").data("kendoDatePicker").value(), "yyyy-MM-dd");
    var pub = "I dont know ok?"
    console.log(id);
    console.log(bookCategory);
    console.log(bookName);
    console.log(bookAuthor);
    console.log(date);
}

function onClick(e) {
    kendoConsole.log("event :: click (" + $(e.event.target).closest(".k-button").attr("id") + ")");
}