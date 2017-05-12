
require(["dojo/data/ItemFileWriteStore", "dojox/grid/DataGrid", "dijit/form/Button", "dijit/Dialog"]);

var orderAction = new OrderAction();
var orderId = "default";
var productList;
function OrderAction() {
    var order = this;

    /* 
    依據篩選條件搜尋訂單
    */
    this.SearchOrderByCondition = function () {
        require(["dojo/data/ItemFileWriteStore", "dojo/dom", "dojo/_base/xhr", "dojo/request", "dojo/dom-form", "dojo/ready", "dojo/json"],
            function (ItemFileWriteStore, dom, xhr, request, domForm, ready) {
                    xhr.post({
                        url: "/Order/GetOrderByCondition",
                        form: dom.byId("searchOrderCondition"),
                        handleAs: "json",
                        load: function (jsonData) {
                            store = new ItemFileWriteStore({ data: jsonData });
                            ready(function () {
                                orderStore.close();
                                orderGrid.setStore(store);
                                orderGrid._refresh();
                                console.log('Success');
                            });
                        },
                        error: function (e) {
                            console.log(e);
                        }
                    });
            }
        );
    }

    /* 
    　將訂單篩選條件設回預設值
    */
    this.resetSearchOrder = function (callback) {
        require(["dojo/data/ItemFileWriteStore", "dojo/dom", "dojo/_base/xhr", "dojo/request", "dojo/dom-form", "dojo/ready", "dojo/json"],
            function (ItemFileWriteStore, dom, xhr, request, domForm, ready) {
                dom.byId("searchOrderCondition").reset();
            }
        );
    }

    /* 
        取得商品清單
    */
    this.GetProductList = function (callback) {
        require(["dojo/data/ItemFileWriteStore", "dojo/dom", "dojo/_base/xhr", "dojo/request", "dojo/dom-form", "dojo/ready", "dojo/json"],
            function (ItemFileWriteStore, dom, xhr, request, domForm, ready) {
                return xhr.get ({
                    url: "/Order/GetProductList",
                    load: callback,
                    error: function (e) {
                        console.log(e);
                    }
                });
            }
        );
    }



    /* 
        紀錄 orderId
    */
    this.setOrderId = function (e) {
        require(["dojo/dom", "dijit/registry", "dojo/dom-class", "dojo/json", "dojo/domReady!"],
                function (dom, registry, domClass) {
                    if (e != null) {
                        var OrderId = orderGrid.getItem(e.rowIndex).OrderId;
                        orderId = OrderId;
                        console.log('onClick:' + orderId);
                    }
                });
    };

    /* 
      依據 item 設定orderId
    */
    this.setOrderIdByItem = function (id) {
        require(["dojo/dom", "dijit/registry", "dojo/dom-class", "dojo/json", "dojo/domReady!"],
                function (dom, registry, domClass) {
                    var OrderId = id;
                    orderId = OrderId;
                    console.log('onClick:' + orderId);
                });
    };

    /* 
      雙擊 grid
    */
    this.dbclickOrderGrid = function (evt) {
        require(["dojo/_base/xhr", "dojo/dom", "dijit/registry", "dojo/domReady!"], function (xhr, dom, registry) {

            orderAction.showOrderUpdateContentPane();

        });
    };

    

    var CircularJSON = function (e, t) { function l(e, t, o) { var u = [], f = [e], l = [e], c = [o ? n : "[Circular]"], h = e, p = 1, d; return function (e, v) { return t && (v = t.call(this, e, v)), e !== "" && (h !== this && (d = p - a.call(f, this) - 1, p -= d, f.splice(p, f.length), u.splice(p - 1, u.length), h = this), typeof v == "object" && v ? (a.call(f, v) < 0 && f.push(h = v), p = f.length, d = a.call(l, v), d < 0 ? (d = l.push(v) - 1, o ? (u.push(("" + e).replace(s, r)), c[d] = n + u.join(n)) : c[d] = c[0]) : v = c[d]) : typeof v == "string" && o && (v = v.replace(r, i).replace(n, r))), v } } function c(e, t) { for (var r = 0, i = t.length; r < i; e = e[t[r++].replace(o, n)]); return e } function h(e) { return function (t, s) { var o = typeof s == "string"; return o && s.charAt(0) === n ? new f(s.slice(1)) : (t === "" && (s = v(s, s, {})), o && (s = s.replace(u, "$1" + n).replace(i, r)), e ? e.call(this, t, s) : s) } } function p(e, t, n) { for (var r = 0, i = t.length; r < i; r++) t[r] = v(e, t[r], n); return t } function d(e, t, n) { for (var r in t) t.hasOwnProperty(r) && (t[r] = v(e, t[r], n)); return t } function v(e, t, r) { return t instanceof Array ? p(e, t, r) : t instanceof f ? t.length ? r.hasOwnProperty(t) ? r[t] : r[t] = c(e, t.split(n)) : e : t instanceof Object ? d(e, t, r) : t } function m(t, n, r, i) { return e.stringify(t, l(t, n, !i), r) } function g(t, n) { return e.parse(t, h(n)) } var n = "~", r = "\\x" + ("0" + n.charCodeAt(0).toString(16)).slice(-2), i = "\\" + r, s = new t(r, "g"), o = new t(i, "g"), u = new t("(?:^|([^\\\\]))" + i), a = [].indexOf || function (e) { for (var t = this.length; t-- && this[t] !== e;); return t }, f = String; return { stringify: m, parse: g } }(JSON, RegExp);
    /* 
      新增訂單資料
    */
    this.InserOrder = function () {
        require(["dojo/data/ItemFileWriteStore", "dojo/dom", "dojo/_base/xhr", "dojo/request", "dojo/dom-form", "dojox/json/ref", "dojo/ready", "dojo/json"],
            function (ItemFileWriteStore, dom, xhr, request, domForm, ref, ready) {
                //console.log(ref.toJson(data.items));
                xhr.post({
                    url: "/Order/InsertOrder",
                    form: dom.byId("insertOrderForm"),
                    handleAs: "json",
                    load: function (jsonData) {
                        console.log(data.items);
                        if (jsonData.State) {
                            xhr.post({
                                url: "/Order/InsertOrderDetail",
                                postData: CircularJSON.stringify({ id: jsonData.Orderid, items: data.items }),
                                headers: { 'Content-Type': 'application/json' },
                                handleAs: "json",
                                load: function (jsonData) {
                                    console.log(jsonData);
                                    if (jsonData.State) {
                                        console.log(jsonData.State);
                                        orderAction.AddSearchProductRow(jsonData.Order.OrderId, jsonData.Order.CustName, jsonData.Order.O_Orderdate, jsonData.Order.O_ShippedDate);
                                        dom.byId("insertOrderForm").reset();
                                        
                                        InsertDialog.hide();
                                    } else {
                                        xhr.post({
                                            url: "/Order/DeleteOrder",
                                            postData: CircularJSON.stringify({ id: jsonData.Orderid }),
                                            headers: { 'Content-Type': 'application/json' },
                                            handleAs: "json",
                                            load: function (jsonData) {
                                                if (jsonData) {
                                                    var rowItem = orderGrid.getItem(deleteOrderRowIndex);
                                                    var store = orderGrid.store;
                                                    //store.deleteItem(rowItem);
                                                    console.log(orderId);
                                                    orderGrid.store.fetch({
                                                        query: { OrderId: orderId },
                                                        onItem: function (item) {
                                                            orderGrid.store.deleteItem(item);
                                                        }
                                                    });
                                                    DeleteDialog.hide();
                                                }
                                                console.log(jsonData);
                                            },
                                            error: function (e) {
                                                console.log(e);
                                            }
                                        });
                                    }
                                    console.log(jsonData);
                                },
                                error: function (e) {
                                    console.log(e);
                                }
                            });
                        } else {
                            console.log(jsonData.Describe);
                            dom.byId('InsertErrorMsg').innerHTML = jsonData.Describe;
                        }
                    },
                    error: function (e) {
                        console.log(e);
                    }
                });
            }
        );
    }

    /* 
      修改訂單資料
    */
    this.UpdateOrder = function () {
        require(["dojo/data/ItemFileWriteStore", "dojo/dom", "dojo/_base/xhr", "dojo/request", "dojo/dom-form", "dojox/json/ref", "dojo/ready", "dojo/json"],
            function (ItemFileWriteStore, dom, xhr, request, domForm, ref, ready) {
                xhr.post({
                    url: "/Order/UpdateOrder",
                    form: dom.byId("updateOrderForm"),
                    handleAs: "json",
                    load: function (jsonData) {
                        console.log(jsonData);
                        if (jsonData.State) {
                            xhr.post({
                                url: "/Order/UpdateOrderDetail",
                                postData: CircularJSON.stringify({ id: orderId, items: updateData.items }),
                                headers: { 'Content-Type': 'application/json' },
                                handleAs: "json",
                                load: function (jsonData) {
                                    if (jsonData) {
                                        dom.byId("updateOrderForm").reset();
                                        UpdateDialog.hide();
                                    }
                                    console.log(jsonData);
                                },
                                error: function (e) {
                                    console.log(e);
                                }
                            });
                        } else {
                            console.log(jsonData.Describe);
                            dom.byId('UpdateErrorMsg').innerHTML = jsonData.Describe;
                        }
                    },
                    error: function (e) {
                        console.log(e);
                    }
                });
            }
        );
    }

    /* 
      刪除訂單資料
    */
    this.DeleteOrder = function () {
        require(["dojox/grid/DataGrid", "dojo/data/ItemFileWriteStore", "dojo/dom", "dojo/_base/xhr", "dojo/request", "dojo/dom-form", "dojox/json/ref", "dojo/ready", "dojo/json"],
            function (DataGrid, ItemFileWriteStore, dom, xhr, request, domForm, ref, ready) {
                //console.log(ref.toJson(data.items));
                xhr.post({
                    url: "/Order/DeleteOrder",
                    postData: CircularJSON.stringify({ id: orderId}),
                    headers: { 'Content-Type': 'application/json' },
                    handleAs: "json",
                    load: function (jsonData) {
                        if (jsonData) {
                            var rowItem = orderGrid.getItem(deleteOrderRowIndex);
                            var store = orderGrid.store;
                            //store.deleteItem(rowItem);
                            console.log(orderId);
                            orderGrid.store.fetch({
                                query: { OrderId: orderId },
                                onItem: function (item) {
                                    orderGrid.store.deleteItem(item);
                                }
                            });
                            DeleteDialog.hide();
                        }
                        console.log(jsonData);
                    },
                    error: function (e) {
                        console.log(e);
                    }
                });
            }
        );
    }



    this.AddProductSum = function (data) {
        require(["dojo/data/ItemFileWriteStore", "dojo/store/Memory", "dojo/dom", "dojo/_base/xhr", "dojo/request", "dojo/dom-form", "dojo/ready", "dojo/json"],
            function (ItemFileWriteStore, Memory, dom, xhr, request, domForm, ready) {
                var total = 0.0;
                for (i in data['items']) {
                    total += parseFloat(data['items'][i].Sum);
                }
                console.log(total);
                dom.byId('Total').innerHTML = total;
            }
        );
    }
    this.AddUpdateProductSum = function (data) {
        require(["dojo/data/ItemFileWriteStore", "dojo/store/Memory", "dojo/dom", "dojo/_base/xhr", "dojo/request", "dojo/dom-form", "dojo/ready", "dojo/json"],
            function (ItemFileWriteStore, Memory, dom, xhr, request, domForm, ready) {
                var total = 0.0;
                for (i in data['items']) {
                    total += parseFloat(data['items'][i].Sum);
                }
                dom.byId('updateTotal').innerHTML = total;

            }
        );
    }
    this.AddSearchProductRow = function (OrderId, CustName, O_Orderdate, O_ShippedDate) {
        require(["dojo/data/ItemFileWriteStore", "dojo/store/Memory", "dojo/dom", "dojo/_base/xhr", "dojo/request", "dojo/dom-form", "dojo/ready", "dojo/json"],
            function (ItemFileWriteStore, Memory, dom, xhr, request, domForm, ready) {
                orderGrid.store.newItem({ OrderId: OrderId, CustName: CustName, O_Orderdate: O_Orderdate, O_ShippedDate: O_ShippedDate });
            }
        );
    }

    this.AddInsertProductRow = function () {
        require(["dojo/data/ItemFileWriteStore", "dojo/store/Memory", "dojo/dom", "dojo/_base/xhr", "dojo/request", "dojo/dom-form", "dojo/ready", "dojo/json"],
            function (ItemFileWriteStore, Memory, dom, xhr, request, domForm, ready) {
                inserProductGrid.store.newItem({ ProductId: 2, ProductName: 1, UnitPrice: 18.00, Qty: 0, Sum: 0 });
            }
        );
    }

    this.AddUpdateProductRow = function () {
        require(["dojo/data/ItemFileWriteStore", "dojo/store/Memory", "dojo/dom", "dojo/_base/xhr", "dojo/request", "dojo/dom-form", "dojo/ready", "dojo/json"],
            function (ItemFileWriteStore, Memory, dom, xhr, request, domForm, ready) {
                updateProductGrid.store.newItem({ ProductId: 2, ProductName: 1, UnitPrice: 18.00, Qty: 0, Sum: 0 });
                updateProductGrid.store.save();
                console.log(updateProductStore);
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

    this.setUpdateProductRowIndex = function (rowIndex) {
        require(["dojo/data/ItemFileWriteStore", "dojo/store/Memory", "dojo/dom", "dojo/_base/xhr", "dojo/request", "dojo/dom-form", "dojo/ready", "dojo/json"],
            function (ItemFileWriteStore, Memory, dom, xhr, request, domForm, ready) {
                updateProductRowIndex = rowIndex;
                console.log(rowIndex);
            }
        );
    }
    this.setDeleteOrderRowIndex = function (rowIndex) {
        require(["dojo/data/ItemFileWriteStore", "dojo/store/Memory", "dojo/dom", "dojo/_base/xhr", "dojo/request", "dojo/dom-form", "dojo/ready", "dojo/json"],
            function (ItemFileWriteStore, Memory, dom, xhr, request, domForm, ready) {
                deleteOrderRowIndex = rowIndex;
                console.log(rowIndex);
            }
        );
    }



    this.showOrderUpdateContentPane = function () {
        require(["dojo/dom", "dojo/_base/xhr", "dijit/registry", "dojo/data/ItemFileWriteStore",
    "dojox/grid/DataGrid", "dojo/json", "dojo/domReady!"],
                function (dom, xhr, registry, ItemFileWriteStore, DataGrid) {
                    if (orderId == 'default') {

                    } else {
                        console.log(updateProductGrid.store);
                        while (updateProductGrid.store._getItemsArray().length != 0) {
                            updateProductGrid.store.deleteItem(updateProductGrid.store._getItemsArray()[0]);
                        }
                        console.log(updateProductGrid.store);
                        xhr.get({
                            url: "/Order/GetUpdateDialog?id=" + orderId,
                            handleAs: "text",
                            load: function (jsonData) {
                                dom.byId('UpdateDailogFormArea').innerHTML = jsonData;
                                dom.byId('UpdateErrorMsg').innerHTML = "";

                                updateProductGrid.store.fetch({
                                    query: { ProductId: 0 },
                                    onItem: function (item) {
                                        updateProductStore.deleteItem(item);
                                    }
                                });
                                xhr.get({
                                    url: "/Order/GetOrderDetailById?id=" + orderId,
                                    handleAs: "text",
                                    load: function (jsonData) {
                                        var orderProduct = JSON.parse(jsonData);
                                        console.log(orderProduct);
                                        for (var i in orderProduct.items) {
                                            updateProductGrid.store.newItem(orderProduct.items[i]);
                                        }
                                        order.AddUpdateProductSum(orderProduct);

                                        updateProductGrid.startup();
                                        updateProductGrid.render();
                                        UpdateDialog.show();
                                    },
                                    error: function (e) {
                                        console.log(e);
                                    }
                                })
                            },
                            error: function (e) {
                                console.log(e);
                            }
                        });

                    }
                });
    };

    this.showOrderInsertContentPane = function () {
        require(["dojo/dom", "dojo/_base/xhr", "dijit/registry", "dojo/data/ItemFileWriteStore",
    "dojox/grid/DataGrid", "dojo/json", "dojo/domReady!"],
                function (dom, xhr, registry, ItemFileWriteStore, DataGrid) {
                    xhr.get({
                        url: "/Order/GetInsertDialog",
                        handleAs: "text",
                        load: function (jsonData) {
                            dom.byId('InsertDailogFormArea').innerHTML = jsonData;
                            dom.byId('InsertErrorMsg').innerHTML = "";
                            InsertDialog.show();
                        },
                        error: function (e) {
                            console.log(e);
                        }
                    });
                });
    };



    this.showOrderDeleteContentPane = function () {
        require(["dojo/dom", "dojo/_base/xhr", "dijit/registry", "dojo/data/ItemFileWriteStore",
    "dojox/grid/DataGrid", "dojo/json", "dojo/domReady!"],
                function (dom, xhr, registry, ItemFileWriteStore, DataGrid) {
                    if (orderId == null) {

                    } else {
                        dom.byId('DeleteDialogContent').innerHTML = "訂單編號:" + orderId;
                        DeleteDialog.show();
                    }
                });
    };

}




//var object;
//orderAction.GetProductList(function (Response) { object = dojo.eval(Response) });
//console.log(orderAction.GetProductList(function (Response) { object = dojo.eval(Response) }));