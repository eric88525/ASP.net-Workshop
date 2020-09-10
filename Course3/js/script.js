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
        $("#book_category").kendoDropDownList({
            dataTextField: "text",
            dataValueField: "value",
            dataSource: data,
            index: 0,
            change: onChange
        });
        $("#bought_datepicker").kendoDatePicker();

     /*   $("#add_button").kendoButton({
            click: addButtonOnclick
        });*/

        $("#show_button").kendoButton({
            click: showButtonOnclick
        });

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

                            var grid = $("#book_grid").data("kendoGrid");

                            grid.removeRow(tr);

                            deleteBook(data.BookId);
                        }
                    },
                    title: " ",
                    width: "120px"
                }

                /*   ,{
                       command: {
                           field: "BookId",
                           text: "刪除",
                           click: deleteBook
                       },
                       title: " ",
                       width: "120px"
                   }*/
            ]

        });

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

   /*     $("#book_name").kendoAutoComplete({
            dataSource: data,
            separator: ", "
        });

        $("#book_author").kendoAutoComplete({
            dataSource: data,
            separator: ", "
        });*/


    }




)




function addButtonOnclick() {
    var id = bookDataFromLocalStorage[bookDataFromLocalStorage.length - 1].BookId;
    var bookCategory = $("#book_category").data("kendoDropDownList").text();
    var bookName = $("#book_name").val();

    for (let i = 0; i < bookDataFromLocalStorage.length; i++) {
        if (bookDataFromLocalStorage[i].BookName == String(bookName)) {
            window.alert("book name duplicate!");
            return;
        }
    }

    var bookAuthor = $("#book_author").val();
    var date = kendo.toString($("#bought_datepicker").data("kendoDatePicker").value(), "yyyy-MM-dd");
    var pub = "I dont know ok?"

  /*  if (bookName == "" || bookAuthor == "") {
        window.alert("book name / bookAuthor empty!");
        return;
    }*/

    var addData = {
        "BookId": Number(id + 1),
        "BookCategory": String(bookCategory),
        "BookName": String(bookName),
        "BookAuthor": String(bookAuthor),
        "BookBoughtDate": String(date),
        "BookPublisher": String(pub)
    }
    bookDataFromLocalStorage.push(addData);
    console.log(addData);
    localStorage.setItem("bookData", JSON.stringify(bookDataFromLocalStorage));

    refreshGrid();
}


function loadBookData() {
    bookDataFromLocalStorage = JSON.parse(localStorage.getItem("bookData"));

    if (bookDataFromLocalStorage == null) {
        bookDataFromLocalStorage = bookData;
        localStorage.setItem("bookData", JSON.stringify(bookDataFromLocalStorage));
    }
}

function onChange() {
    var data = $("#book_category").data("kendoDropDownList");
    $(".book-image").attr("src", "image/" + data.value() + ".jpg");
    console.log(data.text());
}


function filter() {
    bookDataFromLocalStorage = JSON.parse(localStorage.getItem("bookData"));

    bookName = $(".book-grid-search").val();

    var bookfilter = bookDataFromLocalStorage.filter(function (item, index, array) {
        return String(item.BookName).includes(String(bookName));
    });
    console.log(bookName);

    refreshGrid();
}

function refreshGrid() {
    var grid = $("#book_grid").data('kendoGrid');
    grid.dataSource.read();
    grid.refresh();
}

function deleteBook(e) {
    var bookfilter = bookDataFromLocalStorage.filter(function (item, index, array) {
        return item.BookId != e;
    });
    console.log(bookfilter);
    console.log("Details for: " + e);
    bookDataFromLocalStorage = bookfilter;
    localStorage.setItem("bookData", JSON.stringify(bookDataFromLocalStorage));
}

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