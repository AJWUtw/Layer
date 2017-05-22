$(document).ready(function () {

    //搜尋表格設定
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





    // 新增畫面
    $(function () {
        var container = $("#insertForm");
        kendo.init(container);
        container.kendoValidator({
            rules: {
                greaterdate: function (input) {
                    if (input.is("[data-greaterdate-msg]") && input.val() != "") {
                        var date = kendo.parseDate(input.val()),
                            otherDate = kendo.parseDate($("[name='" + input.data("greaterdateField") + "']").val());
                        return otherDate == null || otherDate.getTime() < date.getTime();
                    }

                    return true;
                }
            }
        });
    });

    $("#insert").kendoButton({
        click: insert
    });

    function insert(e) {
        var insertList = $("#insertList").data("kendoGrid");
        var validator = $("#insertForm").data("kendoValidator");

        var data = $('#insertForm').serializeObject();
        data.Products = $("#insertList").data('kendoGrid').dataSource.data().toJSON();

        console.log(data);

        if (validator.validate()) {
            console.log(validator);
            $.ajax({
                url: '../Order2/InsertOrder',
                type: 'POST',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (data) {
                    console.log(data);
                }
            });
        }
    }
    // Customers ComboBox
    $(".customers").kendoComboBox({
        placeholder: "Select Customers",
        dataTextField: "Text",
        dataValueField: "Value",
        //filter: "contains",
        autoBind: false,
        editable: false,
        dataSource: {
            type: "json",
            transport: {
                read: {
                    url: "../Order2/ReadCustomerList",
                }
            }
        }
    });
    $(".customers").attr("readonly", "readonly");

    // Employees ComboBox
    $(".employees").kendoComboBox({
        placeholder: "Select Employees",
        dataTextField: "Text",
        dataValueField: "Value",
        //filter: "contains",
        autoBind: false,
        editable: false,
        dataSource: {
            type: "json",
            transport: {
                read: {
                    url: "../Order2/ReadEmployeeList",
                }
            }
        }
    });
    $(".employees").attr("readonly", "readonly");

    // Shippers ComboBox
    $(".shippers").kendoComboBox({
        readOnly: true,
        placeholder: "Select Shippers",
        dataTextField: "Text",
        dataValueField: "Value",
        //filter: "contains",
        autoBind: false,
        dataSource: {
            type: "json",
            transport: {
                read: {
                    url: "../Order2/ReadShipperList",
                }
            }
        }
    });
    $(".shippers").attr("readonly", "readonly");

    $("#showInertButton").kendoButton({
        click: showInsert
    });

    // 顯示 Insert 畫面

    function showInsert(e) {
        console.log(e);
        $("#insertDialog").data("kendoDialog").open();
    }

    var dialog = $("#insertDialog").kendoDialog({
        width: "90%",
        //height: "90%",
        visible: false,
        title: "訂單資訊",
        closable: true,
        modal: false,
        //content: "<div id='insertList'></div>",
        //actions: [
        //    { text: 'Cancel' },
        //    { text: 'OK', primary: true, action: actionOK }
        //],
        open: function (event, ui) {
            $('body').css('overflow', 'auto');
            $('.ui-widget-overlay').css('width', '90%');
        },
        close: function(event, ui){$('body').css('overflow','auto'); } ,
        initOpen: initOpenInsert
    });


    var insertDataSource = new kendo.data.DataSource({
        transport: {
            read: {
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
                    OrderId: { type: "number", editable: false, nullable: false },
                    ProductId: { type: "number" },
                    ProductName: { defaultValue: { ProductId: 1, ProductName: "Product HHYDP" } },
                    UnitPrice: { type: "number", validation: { min: 0, required: true } },
                    Qty: { type: "number", validation: { min: 0, required: true } },
                    Discount: { type: "number" },
                    Sum: { type: "number", validation: { min: 0, required: true } },
                }
            }
        },
        //serverFiltering: true,
        change: function (e) {
            if (e.action === "itemchange" && e.field !== "Sum") {
                var model = e.items[0],
                    type = model.Type;
                $.get("../Order2/ReadPorductById", { id: model.ProductName.ProductId },
                  function (data) {
                      UnitPrice = data[0].UnitPrice;
                      currentValue = UnitPrice * model.Qty;
                      model.UnitPrice = UnitPrice;
                      $("#insertList").find("tr[data-uid='" + model.uid + "'] td:eq(1)").text('$'+UnitPrice);
                      if (currentValue !== model.Sum) {
                          model.Sum = currentValue;
                          $("#insertList").find("tr[data-uid='" + model.uid + "'] td:eq(3)").text('$' + currentValue);
                      }
                      //$("#insertList").data("kendoGrid").refresh();
                  });
            }
        }
    });

    $("#insertList").kendoGrid({
        dataSource: insertDataSource,
        height: 230,
        filterable: true,
        sortable: true,
        editable: true,
        toolbar: ["create"],
        columns: [
            { field: "ProductName", title: "ProductName", width: "180px", editor: productNameDropDownEditor, template: "#=ProductName.ProductName#" },
            //{
            //    field: "ProductName", title: "ProductName", width: 170
            //},
            { field: "UnitPrice", title: "UnitPrice", width: 110, format: "{0:c2}" },
            { field: "Qty", title: "Qty", width: 110 },
            { field: "Sum", title: "Sum", width: 110, format: "{0:c2}" },
            {
                title: "Edit", command: ["destroy"], width: 80,
                attributes: {
                    style: "text-align: center;"
                }
            }
        ]
    });

    function productNameDropDownEditor(container, options) {
        $('<input required name="' + options.field + '"/>')
            .appendTo(container)
            .kendoDropDownList({
                //valuePrimitive: true,
                autoBind: false,
                dataTextField: "ProductName",
                dataValueField: "ProductId",
                dataSource: {
                    type: "json",
                    transport: {
                        read: "../Order2/ReadPorductList",
                        parameterMap: function (options, operation) {
                            if (operation !== "read" && options.models) {
                                return { models: kendo.stringify(options.models) };
                            }
                        }
                    },
                    schema: {
                        model: {
                            fields: {
                                ProductName: { type: "string" },
                                ProductId: { type: "number" }
                            }
                        }
                    },
                    change: function (e) {
                        if (e.action === "itemchange" && e.field !== "Sum") {
                            var model = e.items[0],
                                type = model.Type;
                            console.log(model);
                            //if (currentValue !== model.Amount) {
                            //    model.Amount = currentValue;
                            //    $("#grid").find("tr[data-uid='" + model.uid + "'] td:eq(4)").text(currentValue);
                            //}
                        }
                    }
                }
            });
    }


    function initOpenInsert(e) {
        setTimeout(function () {
            $("#insertList").data("kendoGrid").refresh();
        })
    }


    function actionOK(e) {
        var insertList = $("#insertList").data("kendoGrid");
        var items = insertList.element.find(".k-state-selected");
        console.log(insertDataSource.data());
        //updateResult(items, updateList);
    }
    $.fn.serializeObject = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name]) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };

});




