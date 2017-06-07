



$(document).ready(function () {

    function Grid() {
        var grid = this;

        this.showInsert = function (e) {
            console.log(e);
            $("#insertDialog").data("kendoDialog").open();
        };


        this.initOpenInsert = function (e) {
            setTimeout(function () {
                $("#insertList").data("kendoGrid").refresh();
            })
        }

        this.insert = function (e) {
            var insertList = $("#insertList").data("kendoGrid");
            var validator = $("#insertForm").data("kendoValidator");

            var data = $('#insertForm').serializeObject();
            data.Products = $("#insertList").data('kendoGrid').dataSource.data().toJSON();
            console.log(validator);
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


        this.search = function (e) {
            $.post("/Order2/GetOrderByCondition", $("#searchOrderCondition").serialize(),
                   function (data) {
                       $('#grid').data("kendoGrid").dataSource.data(data);
                   }, "json");
        }


        this.showUpdateWindow = function (e) {
            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
            console.log(dataItem.OrderId);

            var myWindow = $("#updateWindow");
            myWindow.kendoWindow({
                width: "615px",
                title: "Update",
                content: {
                    url: "./Order2/GetUpdateDialog?id=" + dataItem.OrderId
                }
            });
            console.log(myWindow);
            myWindow.data("kendoWindow").center().open();

        }

        this.productNameDropDownEditor = function (container, options) {
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
                        }
                    }
                });
        }

        this.del = function (orderId) {
            console.log(orderId);
            $.ajax({
                url: '../Order2/DeleteOrder',
                type: 'POST',
                data: JSON.stringify({ 'OrderId': orderId }),
                contentType: 'application/json',
                success: function (data) {
                    console.log(data);
                }
            });
        }

        this.setCombobox = function (action, columns) {
            $("#" + action).kendoComboBox({
                placeholder: "Select " + columns,
                dataTextField: "Text",
                dataValueField: "Value",
                validate: {
                    required: true,
                },
                autoBind: true,
                editable: false,
                dataSource: {
                    type: "json",
                    transport: {
                        read: {
                            url: "../Order2/Read" + columns + "List",
                        }
                    }
                }
            });
            $("#" + action).attr("readonly", "readonly");
        }

    }

    var grid = new Grid();

    //搜尋表格設定
    $("#grid").kendoGrid({
        dataSource: {
            type: "json",
            transport: {
                read: {
                    url: "../Order2/Read",
                    dataType: "json"
                },
                update: function(e) {
                    console.log(e);
                    $.ajax( {
                        url: "../Order2/Update",
                        data: options.data, 
                        success: function(result) {
                            options.success(result);
                        }
                    });
                },
                create: {
                    url: "../Order2/Create",
                    dataType: "jsonp"
                },
                destroy: function(e) {
                    console.log(e);
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
        editable: "popup",
        remove: function (e) {
            grid.del(e.model.OrderId);
        },
        pageable: {
            refresh: true,
            pageSizes: true,
            buttonCount: 5
        },
        showUpdate: function(e){
            console.log(e);
        },
        columns: [{
            field: "CustName", title: "CustName", width: 250,
            editor: function (container, options) {
                    $('<input name="' + options.field + '"/>').appendTo(container).kendoComboBox({
                        dataSource: new kendo.data.DataSource({
                            data: [
                                { Id: "1", title: "Guest" },
                                { Id: "2", title: "Employee" }
                            ]
                        }),
                        dataValueField: "Id",
                        dataTextField: "title",
                        autobind: false
                    });
                }
        }, {
            field: "OrderDate", width: 150,
            template: "#= kendo.toString(kendo.parseDate(OrderDate, 'yyyy-MM-dd'), 'yyyy/MM/dd') #"
        }, {
            field: "ShippedDate", width: 100,
            template: "#= kendo.toString(kendo.parseDate(ShippedDate, 'yyyy-MM-dd'), 'yyyy/MM/dd') #"
        }, {
            command: "destroy" , title: "Delete", width: 110
        }, {
            command: { text: "Update", click: grid.showUpdateWindow }, title: "Update", width: "180px"
        }
        ]
    });


    //Kendo UI ( kendoDatePicker ) : OrderDate / ShippedDate / RequiredDate
    $("#OrderDate").kendoDatePicker({ dateInput: true, format: "yyyy-MM-dd", parseFormats: ["yyyy/MM/dd"] });
    $("#ShippedDate").kendoDatePicker({ dateInput: true, format: "yyyy-MM-dd", parseFormats: ["yyyy/MM/dd"] });
    $("#RequiredDate").kendoDatePicker({ dateInput: true, format: "yyyy-MM-dd", parseFormats: ["yyyy/MM/dd"] });

    // 事件:搜尋
    $("#searchButton").kendoButton({
        click: grid.search
    });
    
    // 事件:新增
    $("#insert").kendoButton({
        click: grid.insert
    });

    // 事件:新增畫面
    $("#showInertButton").kendoButton({
        click: grid.showInsert
    });

    //Kendo UI ( kendoComboBox ) : setCombobox( id, columns) 搜尋  CustName / EmpName / Shipper
    //grid.setCombobox("CustName", "Customer");
    grid.setCombobox("EmpName", "Employee");
    grid.setCombobox("ShipperName", "Shipper");

    //Kendo UI ( kendoComboBox ) : setCombobox( id, columns) 新增  CustName / EmpName / Shipper
    grid.setCombobox("InsertCustName", "Customer");
    grid.setCombobox("InsertEmpName", "Employee");
    grid.setCombobox("InsertShipper", "Shipper");


    //Kendo UI ( kendoDialog ) :  insertDialog 
    $("#insertDialog").kendoDialog({
        width: "90%",
        visible: false,
        title: "訂單資訊",
        closable: true,
        modal: false,
        open: function (event, ui) {
            $('body').css('overflow', 'auto');
            $('.ui-widget-overlay').css('width', '90%');
        },
        close: function(event, ui){$('body').css('overflow','auto'); } ,
        initOpen: grid.initOpenInsert
    });

    //Kendo UI ( kendo DataSource ) :  insertDataSource 
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
                    UnitPrice: { type: "number", validation: { min: 0, required: true }, defaultValue: 18 },
                    Qty: { type: "number", validation: { min: 0, required: true } },
                    Discount: { type: "number" },
                    Sum: { type: "number", validation: { min: 0, required: true } },
                }
            }
        },
        change: function (e) {
            if (e.action === "itemchange" &&  e.field == "ProductName" ) {
                var model = e.items[0],
                    type = model.Type;
                console.log(model);
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
                  });
            } else if (e.action === "itemchange" && (e.field == "UnitPrice" || e.field == "Qty")) {
                var model = e.items[0],
                    type = model.Type;
                UnitPrice = model.UnitPrice;
                currentValue = UnitPrice * model.Qty;
                if (currentValue !== model.Sum) {
                    model.Sum = currentValue;
                    $("#insertList").find("tr[data-uid='" + model.uid + "'] td:eq(3)").text('$' + currentValue);
                }

            }
        }
    });

    //Kendo UI ( kendoGrid ) :  insertList 
    $("#insertList").kendoGrid({
        dataSource: insertDataSource,
        height: 230,
        filterable: true,
        sortable: true,
        editable: true,
        toolbar: ["create"],
        columns: [
            { field: "ProductName", title: "ProductName", width: "180px", editor: grid.productNameDropDownEditor, template: "#=ProductName.ProductName#" },
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



    // 新增畫面驗證
    $(function () {
        var container = $("#insertForm");
        kendo.init(container);
        $("#insertForm").kendoValidator({
            rules: {
                greaterdate: function (input) {
                    console.log(input);
                    if ((input.is("[name=RequiredDate]") || input.is("[name=ShippedDate]")) && input.val() != "") {
                        var date = kendo.parseDate($("[name='" + input[0].name + "']")[1].value),
                            otherDate = kendo.parseDate($("[name='OrderDate']")[1].value);
                        return otherDate == null || otherDate.getTime() < date.getTime();
                    }
                    return true;
                },
                comboboxRequired: function (input) {
                    if (input.is("[name=CustName_input]") || input.is("[name=EmpName_input]") || input.is("[name=ShipperName_input]")) {
                        if (input[0].value == "" ) {
                            return false;
                        }
                    }
                    return true;
                }
            },
            messages: {
                comboboxRequired: "Required",
                greaterdate: "訂購日期需早於送貨日期及要求日期"

            }
        });
    });



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



