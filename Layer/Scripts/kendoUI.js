$(document).ready(function () {
    $("#grid").kendoGrid({
        dataSource: {
            type: "json",
            transport: {
                read: {
                    url: "../Order2/Read",
                    dataType: "json"
                },
                update: function(options) {
                    
                    console.log(options);

                    //$("#ajaxform").submit(function (e) {
                    //    var postData = $(this).serializeArray();
                    //    var formURL = $(this).attr("action");
                    //    $.ajax(
                    //    {
                    //        url: formURL,
                    //        type: "POST",
                    //        data: postData,
                    //        success: function (data, textStatus, jqXHR) {
                    //            //data: return data from server
                    //        },
                    //        error: function (jqXHR, textStatus, errorThrown) {
                    //            //if fails      
                    //        }
                    //    });
                    //    e.preventDefault(); //STOP default action
                    //    e.unbind(); //unbind. to stop multiple form submit.
                    //});

                    //$("#ajaxform").submit();

                    $.ajax( {
                        url: "../Order2/Update",
                        data: options.data, // the "data" field contains paging, sorting, filtering and grouping data
                        success: function(result) {
                            // notify the DataSource that the operation is complete

                            options.success(result);
                        }
                    });
                },
                create: {
                    url: "../Order2/Create",
                    dataType: "jsonp"
                },
                destroy: {
                    url: "../Order2/Destroy",
                    dataType: "jsonp"
                },
                parameterMap: function (options, operation) {
                    if (operation !== "read" && options.models) {
                        return { models: kendo.stringify(options.models) };
                    }
                }
            },
            pageSize: 20
        },
        height: 550,
        groupable: true,
        sortable: true,
        editable: false,
        toolbar: ["create", "save", "cancel"],
        pageable: {
            refresh: true,
            pageSizes: true,
            buttonCount: 5
        },
        showUpdate: function(e){
            console.log(e);
        },
        columns: [{
            field: "CustName",
            title: "CustName",
            width: 250
        }, {
            //    field: "UnitPrice",
            //    title: "UnitPrice",
            //    format: "{0:c}",
            //    width: 150
            //}, {
            field: "Orderdate",
            width: 150,
            template: "#= kendo.toString(kendo.parseDate(Orderdate, 'yyyy-MM-dd'), 'yyyy/MM/dd') #"
        }, {
            field: "ShippedDate",
            width: 100,
            template: "#= kendo.toString(kendo.parseDate(ShippedDate, 'yyyy-MM-dd'), 'yyyy/MM/dd') #"
        }, {
            command: { text: "Update", click: showUpdate },
            title: "Update",
            width: 110
        }, {
            command: "destroy",
            title: "Delete",
            width: 110
        }
        ]
    });

    // 依條件搜尋
    $("#searchButton").kendoButton({
        click: onClick
    });

    function onClick(e) {
        $.post("/Order2/GetOrderByCondition", $("#searchOrderCondition").serialize(),
               function (data) {
                   console.log(data);
                   $('#grid').data("kendoGrid").dataSource.data( data );
               }, "json");
        
        //kendoConsole.log("event :: click (" + $(e.event.target).closest(".k-button").attr("id") + ")");

    }

    // 顯示 Update 畫面

    function showUpdate(e) {
        console.log(e);
        $("#updateDialog").data("kendoDialog").open();
    }

    var dialog = $("#updateDialog").kendoDialog({
        width: "800px",
        height: "520px",
        visible: false,
        title: "訂單資訊",
        closable: true,
        modal: false,
        content: "<div id='updateList'></div>",
        actions: [
            { text: 'Cancel' },
            { text: 'OK', primary: true, action: actionOK }
        ],
        initOpen: initOpen
    });


    var updateDataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: "../Order2/GetOrderDetailById?id=10643",
                dataType: "json"
            },
            parameterMap: function (options, operation) {
                if (operation !== "read" && options.models) {
                    return { models: kendo.stringify(options.models) };
                }
            }
        },
        batch: true,
        schema: {
            model: {
                id: "OrderId",
                fields: {
                    //EmployeeId: { type: "number", editable: false, nullable: false },
                    //ReportsTo: { nullable: true, type: "number" },
                    //FirstName: { validation: { required: true } },
                    //LastName: { validation: { required: true } },
                    //HireDate: { type: "date" },
                    //Phone: { type: "string" },
                    //HireDate: { type: "date" },
                    //BirthDate: { type: "date" },
                    //Extension: { type: "number", validation: { min: 0, required: true } },
                    //Position: { type: "string" }
                    OrderId: { type: "number", editable: false, nullable: false },
                    ProductId: { type: "number" },
                    ProductName: { type: "number" },
                    UnitPrice: { type: "number", validation: { min: 0, required: true } },
                    Qty: { type: "number", validation: { min: 0, required: true } },
                    Discount: { type: "number"},
                    Sum: { type: "number", validation: { min: 0, required: true } },
                }
            }
        }
    });

    $("#updateList").kendoGrid({
        dataSource: updateDataSource,
        height: 380,
        filterable: true,
        sortable: true,
        editable: true,
        toolbar: ["create"],
        columns: [
            //{
            //    headerTemplate: "<input type='checkbox' onclick='toggleAll(event)' />",
            //    template: "<input type='checkbox' class='checkbox' data-bind='checked: checked' />",
            //    width: 40,
            //    filterable: false
            //},
            {
                field: "ProductName", title: "ProductName", width: 170
            },
            { field: "UnitPrice", title: "UnitPrice", width: 110, format: "{0:c2}" },
            { field: "Qty", title: "UnitPrice", width: 110 },
            { field: "Sum", title: "UnitPrice", width: 110, format: "{0:c2}" },
            {
                title: "Edit", command: ["destroy"], width: 80,
                attributes: {
                    style: "text-align: center;"
                }
            }
        ]
    });        


    function initOpen(e) {
        setTimeout(function () {
            $("#updateList").data("kendoGrid").refresh();
        })
    }


    function actionOK(e) {
        var updateList = $("#updateList").data("kendoGrid");
        var items = updateList.element.find(".k-state-selected");
        console.log(updateDataSource.data());
        //updateResult(items, updateList);
    }

});