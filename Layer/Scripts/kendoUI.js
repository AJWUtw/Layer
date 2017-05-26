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
        editable: {
            mode: "popup",
            template: $("#orderTemplate").html(),
            window: {
                resizable: true,
                modal: true,
                draggable: true,
                visible: true
            },
            createAt: "center"
        },
        edit: function (e) {
            var editWindow = this.editable.element.data("kendoWindow");
            if (editWindow) {
                editWindow.wrapper.css({
                    width: "800px"
                });
                editWindow.center();
            }
            $("#updateCustName").kendoComboBox({
                placeholder: "Select Customers",
                dataTextField: "Text",
                dataValueField: "Value",
                //filter: "contains",
                autoBind: true,
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
            $("#updateCustName").attr("readonly", "readonly");

            // Employees ComboBox
            $("#updateEmpName").kendoComboBox({
                placeholder: "Select Employees",
                dataTextField: "Text",
                dataValueField: "Value",
                dataBind: "Text",
                autoBind: true,
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
            $("#updateEmpName").attr("readonly", "readonly");

            // Shippers ComboBox
            $("#updateShipperName").kendoComboBox({
                readOnly: true,
                placeholder: "Select Shippers",
                dataTextField: "Text",
                dataValueField: "Value",
                dataBind: "Text",
                autoBind: true,
                dataSource: {
                    type: "json",
                    transport: {
                        read: {
                            url: "../Order2/ReadShipperList",
                        }
                    }
                }
            });
            $("#updateShipperName").attr("readonly", "readonly");

            $("#updateShipperName").data('kendoComboBox').value(e.model.ShipperId);
            $("#updateEmpName").data('kendoComboBox').value(e.model.EmpId);
            $("#updateCustName").data('kendoComboBox').value(e.model.CustId);
            setUpdateDataSource(e.model.OrderId);

            //var editWindow = $("#GridPopUp").data("kendoWindow");
            //if (editWindow) {
            //    editWindow.center();
            //}

            //$(e.container).parent().css({
            //    width: '600px',
            //    left: "10%"
            //});
        },
        save: function (e) {
            update(e);
        },
        remove: function (e) {
            del(e.model.OrderId);
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
            //command: { text: "Update", click: showUpdate }, title: "Update", width: 110
            command: "edit" , title: "Update", width: 110
        }, {
            command: "destroy", title: "Delete", width: 110
        }]
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


    function update(e) {
        var updateList = $("#updateList").data("kendoGrid");
        var validator = $("#updateForm").data("kendoValidator");

        var data = $('#updateForm').serializeObject();
        data.Products = $("#updateList").data('kendoGrid').dataSource.data().toJSON();
        data.OrderId = e.model.OrderId;
        if (validator.validate()) {
            $.ajax({
                url: '../Order2/UpdateOrder',
                type: 'POST',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (data) {

                    if (data) {
                        var window = $("#window").data("kendoWindow");

                    } else {

                    }
                }
            });
        }
    }


    function del(orderId) {
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

    // Customers ComboBox
    $(".customers").kendoComboBox({
        placeholder: "Select Customers",
        dataTextField: "Text",
        dataValueField: "Value",
        //filter: "contains",
        autoBind: true,
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
        autoBind: true,
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
        dataBind: "Text",
        //filter: "contains",
        autoBind: true,
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
        //$("#insertContent").html($("#orderTemplate").html());
        $("#insertDialog").data("kendoDialog").open();
    }

    var dialog = $("#insertDialog").kendoDialog({
        width: "90%",
        //height: "90%",
        visible: false,
        title: "訂單資訊",
        closable: true,
        modal: false,
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
                    }
                }
            });
    }


    function initOpenInsert(e) {
        setTimeout(function () {
            $("#insertList").data("kendoGrid").refresh();
        })
    }

    // Update

    function setUpdateDataSource(orderId) {
        var updateDataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    dataType: "json",
                    url: "../Order2/GetOrderDetailById?id=" + orderId,
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
                        OrderId: { type: "number" },
                        ProductId: { type: "number" },
                        ProductName: { defaultValue: { ProductId: 1, ProductName: "Product HHYDP" } },
                        UnitPrice: { type: "number", validation: { min: 0, required: true } },
                        Qty: { type: "number", validation: { min: 0, required: true } },
                        Discount: { type: "number" },
                        Sum: { type: "number", validation: { min: 0, required: true } },
                    }
                }
            },
            change: function (e) {
                if (e.action === "itemchange" && e.field !== "Sum") {
                    var model = e.items[0],
                        type = model.Type;
                    console.log(model);
                    $.get("../Order2/ReadPorductById", { id: model.ProductName.ProductId },
                      function (data) {
                          UnitPrice = data[0].UnitPrice;
                          currentValue = UnitPrice * model.Qty;
                          model.UnitPrice = UnitPrice;
                          $("#updateList").find("tr[data-uid='" + model.uid + "'] td:eq(1)").text('$' + UnitPrice);
                          if (currentValue !== model.Sum) {
                              model.Sum = currentValue;
                              $("#updateList").find("tr[data-uid='" + model.uid + "'] td:eq(3)").text('$' + currentValue);
                          }
                      });
                }
            }
        });

        $("#updateList").kendoGrid({
            dataSource: updateDataSource,
            height: 230,
            width: "100%",
            filterable: true,
            sortable: true,
            editable: true,
            toolbar: ["create"],
            columns: [
                { field: "ProductName", title: "ProductName", width: "180px", editor: productNameDropDownEditor, template: "#=ProductName.ProductName#" },
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
    }
    

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




