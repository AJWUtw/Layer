

var searchStructure = [
                            {
                                name: "訂單編號",
                                field: "OrderId"
                            },
                            {
                                name: "客戶名稱",
                                field: "CustName"
                            },
                            {
                                name: "訂購日期",
                                field: "O_Orderdate"
                            },
                            {
                                name: "發貨日期",
                                field: "O_ShippedDate"
                            },
                            {
                                name: "修改",
                                field: "OrderId",
                                formatter: function (item) {
                                    var btn = new dijit.form.Button({
                                        label: "Edit",
                                        onClick: function () {
                                            orderAction.setOrderIdByItem(item);
                                            //console.log(evt.rowNode.gridRowIndex);
                                            orderAction.dbclickOrderGrid();
                                        }
                                    });
                                    return btn;
                                }
                            },
                            {
                                name: "刪除",
                                field: "OrderId",
                                formatter: function (item) {
                                    var btn = new dijit.form.Button({
                                        label: "Delete"
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
var productName;

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
        { ProductId: 1, ProductName: "Product HHYDP", UnitPrice: 18.00, Qty: 0, Sum: 0 }
        ]
    };
    

    pNameStore = new ItemFileWriteStore({ url: '/Order/GetProductNameList' });
    insertProductStore = new ItemFileWriteStore({ data: data });

    xhr.get({
        url: "/Order/GetProductList",
        handleAs: "json",
        load: function (jsonData) {
            console.log(jsonData[0]);
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

                                            var item = inserProductGrid.getItem(this.tabIndex);
                                            console.log(this);
                                            var rowItem = inserProductGrid.getItem(insertProductRowIndex);
                                            var qty = insertProductStore.getValue(rowItem, "Qty");
                                            insertProductStore.setValue(rowItem, 'ProductName', jsonData[evt-1].ProductId);
                                            insertProductStore.setValue(rowItem, 'UnitPrice', jsonData[evt-1].UnitPrice);
                                            insertProductStore.setValue(rowItem, 'Sum', qty * jsonData[evt - 1].UnitPrice);
                                            addProductSum(data);
                                        }
                                    },
                                    formatter: function (inDatum, inRowIndex) {
                                        //console.log(inDatum);
                                        //console.log(inRowIndex);
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
                                            addProductSum(data);
                                            
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
                    gridAction.setInsertProductRowIndex(evt.rowIndex);
                }
            }, "inserProductGrid");
            inserProductGrid.startup();
        },
        error: function (e) {
            console.log(e);
        }
    });
});



var gridAction = new GridAction();
var productList;
function GridAction() {
    var order = this;

    this.AddInsertProductRow = function () {
        require(["dojo/data/ItemFileWriteStore", "dojo/store/Memory", "dojo/dom", "dojo/_base/xhr", "dojo/request", "dojo/dom-form", "dojo/ready", "dojo/json"],
            function (ItemFileWriteStore, Memory, dom, xhr, request, domForm, ready) {
                inserProductGrid.store.newItem({ ProductId: 2, ProductName: "Product HHYDP", UnitPrice: 18.00, Qty: 0, Sum: 0 });
            }
        );
    }

    this.setInsertProductRowIndex = function (rowIndex) {
        require(["dojo/data/ItemFileWriteStore", "dojo/store/Memory", "dojo/dom", "dojo/_base/xhr", "dojo/request", "dojo/dom-form", "dojo/ready", "dojo/json"],
            function (ItemFileWriteStore, Memory, dom, xhr, request, domForm, ready) {
                insertProductRowIndex = rowIndex;
                console.log(rowIndex);
            }
        );
    }
}