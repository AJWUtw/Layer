

var searchStructure = [
                            {
                                name: "訂單編號",
                                field: "OrderId",
                                width: 'auto'
                            },
                            {
                                name: "客戶名稱",
                                field: "CustName",
                                width: 'auto'
                            },
                            {
                                name: "訂購日期",
                                field: "O_Orderdate",
                                width: 'auto'
                            },
                            {
                                name: "發貨日期",
                                field: "O_ShippedDate",
                                width: 'auto'
                            },
                            {
                                name: "修改",
                                field: "OrderId",
                                width: '80px',
                                formatter: function (item) {
                                    var btn = new dijit.form.Button({
                                        label: "Edit",
                                        onClick: function () {
                                            orderAction.setOrderIdByItem(item);
                                            orderAction.dbclickOrderGrid();
                                        },
                                        iconClass: "dijitIconEdit"
                                    });
                                    return btn;
                                }

                            },
                            {
                                name: "刪除",
                                field: "OrderId",
                                width: '80px',
                                formatter: function (item,rowIndex) {
                                    var btn = new dijit.form.Button({
                                        label: "Delete",
                                        onClick: function (evt) {
                                            console.log(rowIndex);
                                            orderAction.setOrderIdByItem(item);
                                            orderAction.setDeleteOrderRowIndex(rowIndex);
                                            orderAction.showOrderDeleteContentPane();
                                        },
                                        iconClass: "dijitIconDelete"
                                    });
                                    return btn;
                                }
                            }
]


var insertStructure;
var insertStore;
var data;
var insertProductStore;
var insertProductRowIndex;
var qty;


var updateStructure;
var updateStore;
var updateData;
var updateProductStore;
var updateProductRowIndex;
var updateQty;

var deleteOrderRowIndex;


require([
    "dojo/dom",
    "dojox/grid/DataGrid",
    "dojox/grid/cells",
    "dojox/grid/cells/dijit",
    "dojo/data/ItemFileWriteStore",
    "dojo/currency",
    "dijit/form/CurrencyTextBox",
    "dojo/_base/xhr",
    "dijit/form/NumberSpinner",
    "dijit/form/FilteringSelect",
    "dojo/domReady!"
], function (dom, DataGrid, cells, cellsDijit, ItemFileWriteStore, currency,
             CurrencyTextBox, xhr, NumberSpinner, FilteringSelect) {
    var grid, gridLayout;
    function formatCurrency(inDatum) {
        return isNaN(inDatum) ? '...' : currency.format(inDatum, this.constraint);
    }
    function formatDate(inDatum) {
        return locale.format(new Date(inDatum), this.constraint);
    }
    function addProductSum(data) {
        var total = 0.0;
        for (i in data['items']) {
            total += parseFloat(data['items'][i].Sum);
        }
        console.log(total);
        dom.byId('Total').innerHTML = total;
    }
    

    data = {
        identifer: 'ProductId',
        items: [
        { ProductId: 1, ProductName: 1, UnitPrice: 18.00, Qty: 0, Sum: 0 }
        ]
    };
    

    pNameStore = new ItemFileWriteStore({ url: '/Order/GetProductNameList' });
    insertProductStore = new ItemFileWriteStore({ data: data });


    //Update

    function addUpdateProductSum(data) {
        var total = 0.0;
        for (i in data['items']) {
            total += parseFloat(data['items'][i].Sum);
        }
        console.log(total);
        dom.byId('updateTotal').innerHTML = total;

    }

    updateData = {
        identifer: 'ProductId',
        items: [
        { ProductId: 0, ProductName: "Product HHYDP", UnitPrice: 18.00, Qty: 0, Sum: 0 }
        ]
    };

    updateProductStore = new ItemFileWriteStore({ data: updateData });



    xhr.get({
        url: "/Order/GetProductList",
        handleAs: "json",
        load: function (jsonData) {
            
            gridLayout = [{
                defaultCell: { width: 8, editable: true, type: cells._Widget, styles: 'text-align: right;' },
                cells: [
                                {
                                    name: '商品', 
                                    styles: 'text-align: center;',
                                    field: 'ProductName',
                                    width: 10,
                                    type: dojox.grid.cells._Widget, 
                                    widgetClass: dijit.form.FilteringSelect,
                                    widgetProps: {
                                        store: pNameStore,
                                        onChange: function (evt) {
                                            var rowItem = inserProductGrid.getItem(insertProductRowIndex);
                                            var qty = insertProductStore.getValue(rowItem, "Qty");
                                            insertProductStore.setValue(rowItem, 'ProductName', jsonData[evt-1].ProductId);
                                            insertProductStore.setValue(rowItem, 'UnitPrice', jsonData[evt-1].UnitPrice);
                                            insertProductStore.setValue(rowItem, 'Sum', qty * jsonData[evt - 1].UnitPrice);
                                            orderAction.AddProductSum(data);
                                        }
                                    },
                                    formatter: function (inDatum, inRowIndex) {
                                        var sName = "Product HHYDP";
                                        pNameStore.fetchItemByIdentity({
                                            identity: inDatum,
                                            onItem: function (item) {
                                                if (item) sName = item.name[0];
                                            }
                                        });
                                        return sName;
                                    }
                                },
                                {
                                    name: "單價",
                                    field: "UnitPrice",
                                    editable: false,
                                    formatter: formatCurrency,
                                    constraint: { currency: 'TWD' },
                                    width: 10
                                },
                                {
                                    name: "數量",
                                    field: "Qty",
                                    editable: true,
                                    type: dojox.grid.cells._Widget,
                                    widgetClass: dijit.form.NumberSpinner,
                                    widgetProps: {
                                        value: 2,
                                        smallDelta: 1,
                                        onChange: function (evt) {
                                            console.log(evt);
                                            var rowItem = inserProductGrid.getItem(insertProductRowIndex);
                                            var unitPrice = insertProductStore.getValue(rowItem, "UnitPrice");
                                            insertProductStore.setValue(rowItem, 'Qty', evt);
                                            insertProductStore.setValue(rowItem, 'Sum', unitPrice * evt);
                                            orderAction.AddProductSum(data);
                                            
                                        }
                                    }
                                },
                                {
                                    name: "小計",
                                    field: "Sum",
                                    editable: false,
                                    formatter: formatCurrency,
                                    constraint: { currency: 'TWD' },
                                    width: 10,
                                    onChange: function (evt) {
                                        console.log(evt);
                                    }
                                },
                                {
                                    name: "取消",
                                    field: "ProductId",
                                    formatter: function (item,rowIndex) {
                                        var btn = new dijit.form.Button({
                                            label: "取消",
                                            onClick: function () {
                                                var rowItem = inserProductGrid.getItem(rowIndex);
                                                insertProductStore.deleteItem(rowItem);
                                                orderAction.AddProductSum(data);
                                            }
                                        });
                                        return btn;
                                    }
                                }
                ]
            }];
            inserProductGrid = new DataGrid({
                store: insertProductStore,
                structure: gridLayout,
                "class": "grid",
                autoWidth: "true",
                autoHeight: "true",
                onClick: function (evt, rowIndex) {
                    orderAction.setInsertProductRowIndex(evt.rowIndex);
                }
            }, "inserProductGrid");
            inserProductGrid.startup();

            //Update
            
            updateGridLayout = [{
                defaultCell: { width: 8, editable: true, type: cells._Widget, styles: 'text-align: right;' },
                cells: [
                                {
                                    name: '商品',
                                    styles: 'text-align: center;',
                                    field: 'ProductName',
                                    width: 10,
                                    type: dojox.grid.cells._Widget,
                                    widgetClass: dijit.form.FilteringSelect,
                                    widgetProps: {
                                        store: pNameStore,
                                        onChange: function (evt) {
                                            console.log(this);
                                            var rowItem = updateProductGrid.getItem(updateProductRowIndex);
                                            var qty = updateProductStore.getValue(rowItem, "Qty");
                                            updateProductStore.setValue(rowItem, 'ProductName', jsonData[evt - 1].ProductId);
                                            updateProductStore.setValue(rowItem, 'UnitPrice', jsonData[evt - 1].UnitPrice);
                                            updateProductStore.setValue(rowItem, 'Sum', qty * jsonData[evt - 1].UnitPrice);
                                            orderAction.AddUpdateProductSum(updateData);
                                        }
                                    },
                                    formatter: function (inDatum, inRowIndex) {
                                        var sName = "Product HHYDP";
                                        pNameStore.fetchItemByIdentity({
                                            identity: inDatum,
                                            onItem: function (item) {
                                                if (item) sName = item.name[0];
                                            }
                                        });
                                        return sName;
                                    }
                                },
                                {
                                    name: "單價",
                                    field: "UnitPrice",
                                    editable: false,
                                    formatter: formatCurrency,
                                    constraint: { currency: 'TWD' },
                                    width: 10
                                },
                                {
                                    name: "數量",
                                    field: "Qty",
                                    editable: true,
                                    type: dojox.grid.cells._Widget,
                                    widgetClass: dijit.form.NumberSpinner,
                                    widgetProps: {
                                        value: 2,
                                        smallDelta: 1,
                                        onChange: function (evt) {
                                            console.log(evt);
                                            var rowItem = updateProductGrid.getItem(updateProductRowIndex);
                                            var unitPrice = updateProductStore.getValue(rowItem, "UnitPrice");
                                            updateProductStore.setValue(rowItem, 'Qty', evt);
                                            updateProductStore.setValue(rowItem, 'Sum', unitPrice * evt);
                                            orderAction.AddUpdateProductSum(updateData);

                                        }
                                    }
                                },
                                {
                                    name: "小計",
                                    field: "Sum",
                                    editable: false,
                                    formatter: formatCurrency,
                                    constraint: { currency: 'TWD' },
                                    width: 10,
                                    onChange: function (evt) {
                                        console.log(evt);
                                    }
                                },
                                {
                                    name: "取消",
                                    field: "ProductId",
                                    formatter: function (item, rowIndex) {
                                        var btn = new dijit.form.Button({
                                            label: "取消",
                                            onClick: function () {
                                                var rowItem = updateProductGrid.getItem(rowIndex);
                                                updateProductStore.deleteItem(rowItem);
                                                orderAction.AddUpdateProductSum(updateData);
                                            }
                                        });
                                        return btn;
                                    }
                                }
                ]
            }];

            updateProductGrid = new DataGrid({
                store: updateProductStore,
                structure: updateGridLayout,
                "class": "grid",
                autoWidth: "true",
                autoHeight: "true",
                onClick: function (evt, rowIndex) {
                    orderAction.setUpdateProductRowIndex(evt.rowIndex);
                }
            }, "updateProductGrid");



        },
        error: function (e) {
            console.log(e);
        }
    });
});
var orderGrid;
var orderStore;
require([
    "dojo/dom",
    "dojo/data/ItemFileWriteStore",
    "dojox/grid/EnhancedGrid",
    "dojox/grid/enhanced/plugins/Pagination",
    "dojo/domReady!"
], function (dom, ItemFileWriteStore, EnhancedGrid, Pagination) {
    
    
    orderStore = new ItemFileWriteStore({ url: './GetOrder' });

    orderGrid = new EnhancedGrid({
        id: 'grid',
        store: orderStore,
        structure: searchStructure,
        rowSelector: '20px',
        autoHeight:"true",
        Width: "100%",
        rowWidth: "100%",
        plugins: {
            pagination: {
                pageSizes: ["25", "50", "100", "All"],
                description: true,
                sizeSwitch: true,
                pageStepper: true,
                gotoButton: true,
                maxPageStep: 4,
                position: "bottom"
            }
        },
        onClick: function (evt, rowIndex) {
            orderAction.setOrderId();
        }
    }, gridDiv);

    orderGrid.startup();
});




var gridAction = new GridAction();
var productList;
function GridAction() {
    var grid = this;

    //this.AddProductSum = function (data) {
    //    require(["dojo/data/ItemFileWriteStore", "dojo/store/Memory", "dojo/dom", "dojo/_base/xhr", "dojo/request", "dojo/dom-form", "dojo/ready", "dojo/json"],
    //        function (ItemFileWriteStore, Memory, dom, xhr, request, domForm, ready) {
    //            var total = 0.0;
    //            for (i in data['items']) {
    //                total += parseFloat(data['items'][i].Sum);
    //            }
    //            console.log(total);
    //            dom.byId('Total').innerHTML = total;
    //        }
    //    );
    //}
    //this.AddUpdateProductSum = function (data) {
    //    require(["dojo/data/ItemFileWriteStore", "dojo/store/Memory", "dojo/dom", "dojo/_base/xhr", "dojo/request", "dojo/dom-form", "dojo/ready", "dojo/json"],
    //        function (ItemFileWriteStore, Memory, dom, xhr, request, domForm, ready) {
    //            var total = 0.0;
    //            for (i in data['items']) {
    //                total += parseFloat(data['items'][i].Sum);
    //            }
    //            dom.byId('updateTotal').innerHTML = total;
                
    //        }
    //    );
    //}
    //this.AddSearchProductRow = function (OrderId, CustName, O_Orderdate, O_ShippedDate) {
    //    require(["dojo/data/ItemFileWriteStore", "dojo/store/Memory", "dojo/dom", "dojo/_base/xhr", "dojo/request", "dojo/dom-form", "dojo/ready", "dojo/json"],
    //        function (ItemFileWriteStore, Memory, dom, xhr, request, domForm, ready) {
    //            orderGrid.store.newItem({ OrderId: OrderId, CustName: CustName, O_Orderdate: O_Orderdate, O_ShippedDate: O_ShippedDate });
    //        }
    //    );
    //}

    //this.AddInsertProductRow = function () {
    //    require(["dojo/data/ItemFileWriteStore", "dojo/store/Memory", "dojo/dom", "dojo/_base/xhr", "dojo/request", "dojo/dom-form", "dojo/ready", "dojo/json"],
    //        function (ItemFileWriteStore, Memory, dom, xhr, request, domForm, ready) {
    //            inserProductGrid.store.newItem({ ProductId: 2, ProductName: 1, UnitPrice: 18.00, Qty: 0, Sum: 0 });
    //        }
    //    );
    //}

    //this.AddUpdateProductRow = function () {
    //    require(["dojo/data/ItemFileWriteStore", "dojo/store/Memory", "dojo/dom", "dojo/_base/xhr", "dojo/request", "dojo/dom-form", "dojo/ready", "dojo/json"],
    //        function (ItemFileWriteStore, Memory, dom, xhr, request, domForm, ready) {
    //            updateProductGrid.store.newItem({ ProductId: 2, ProductName: 1, UnitPrice: 18.00, Qty: 0, Sum: 0 });
    //            updateProductGrid.store.save();
    //            console.log(updateProductStore);
    //        }
    //    );
    //}


    //this.setInsertProductRowIndex = function (rowIndex) {
    //    require(["dojo/data/ItemFileWriteStore", "dojo/store/Memory", "dojo/dom", "dojo/_base/xhr", "dojo/request", "dojo/dom-form", "dojo/ready", "dojo/json"],
    //        function (ItemFileWriteStore, Memory, dom, xhr, request, domForm, ready) {
    //            insertProductRowIndex = rowIndex;
    //            console.log(rowIndex);
    //        }
    //    );
    //}

    //this.setUpdateProductRowIndex = function (rowIndex) {
    //    require(["dojo/data/ItemFileWriteStore", "dojo/store/Memory", "dojo/dom", "dojo/_base/xhr", "dojo/request", "dojo/dom-form", "dojo/ready", "dojo/json"],
    //        function (ItemFileWriteStore, Memory, dom, xhr, request, domForm, ready) {
    //            updateProductRowIndex = rowIndex;
    //            console.log(rowIndex);
    //        }
    //    );
    //}
    //this.setDeleteOrderRowIndex = function (rowIndex) {
    //    require(["dojo/data/ItemFileWriteStore", "dojo/store/Memory", "dojo/dom", "dojo/_base/xhr", "dojo/request", "dojo/dom-form", "dojo/ready", "dojo/json"],
    //        function (ItemFileWriteStore, Memory, dom, xhr, request, domForm, ready) {
    //            deleteOrderRowIndex = rowIndex;
    //            console.log(rowIndex);
    //        }
    //    );
    //}



    //this.showOrderUpdateContentPane = function () {
    //    require(["dojo/dom", "dojo/_base/xhr", "dijit/registry", "dojo/data/ItemFileWriteStore",
    //"dojox/grid/DataGrid", "dojo/json", "dojo/domReady!"],
    //            function (dom, xhr, registry, ItemFileWriteStore, DataGrid) {
    //                if (orderId == 'default') {

    //                } else {
    //                    console.log(updateProductGrid.store);
    //                    while (updateProductGrid.store._getItemsArray().length != 0) {
    //                        updateProductGrid.store.deleteItem(updateProductGrid.store._getItemsArray()[0]);
    //                    }
    //                    console.log(updateProductGrid.store);
    //                    xhr.get({
    //                        url: "/Order/GetUpdateDialog?id=" + orderId,
    //                        handleAs: "text",
    //                        load: function (jsonData) {
    //                            dom.byId('UpdateDailogFormArea').innerHTML = jsonData;

    //                            updateProductGrid.store.fetch({
    //                                query: { ProductId: 0 },
    //                                onItem: function (item) {
    //                                    updateProductStore.deleteItem(item);
    //                                }
    //                            });
    //                            xhr.get({
    //                                url: "/Order/GetOrderDetailById?id=" + orderId,
    //                                handleAs: "text",
    //                                load: function (jsonData) {
    //                                    var orderProduct = JSON.parse(jsonData);

    //                                    for (var i in orderProduct.items) {
    //                                        updateProductGrid.store.newItem(orderProduct.items[i]);
    //                                    }
    //                                    grid.AddUpdateProductSum(orderProduct);

    //                                    updateProductGrid.startup();
    //                                    UpdateDialog.show();
    //                                },
    //                                error: function (e) {
    //                                    console.log(e);
    //                                }
    //                            })
    //                        },
    //                        error: function (e) {
    //                            console.log(e);
    //                        }
    //                    });

    //                }
    //            });
    //};

    //this.showOrderInsertContentPane = function () {
    //    require(["dojo/dom", "dojo/_base/xhr", "dijit/registry", "dojo/data/ItemFileWriteStore",
    //"dojox/grid/DataGrid", "dojo/json", "dojo/domReady!"],
    //            function (dom, xhr, registry, ItemFileWriteStore, DataGrid) {
    //                xhr.get({
    //                    url: "/Order/GetInsertDialog",
    //                    handleAs: "text",
    //                    load: function (jsonData) {
    //                        dom.byId('InsertDailogFormArea').innerHTML = jsonData;
    //                        InsertDialog.show();
    //                    },
    //                    error: function (e) {
    //                        console.log(e);
    //                    }
    //                });
    //            });
    //};



    //this.showOrderDeleteContentPane = function () {
    //    require(["dojo/dom", "dojo/_base/xhr", "dijit/registry", "dojo/data/ItemFileWriteStore",
    //"dojox/grid/DataGrid", "dojo/json", "dojo/domReady!"],
    //            function (dom, xhr, registry, ItemFileWriteStore, DataGrid) {
    //                if (orderId == null) {

    //                } else {
    //                    dom.byId('DeleteDialogContent').innerHTML = "訂單編號:" + orderId;
    //                    DeleteDialog.show();
    //                }
    //            });
    //};

}