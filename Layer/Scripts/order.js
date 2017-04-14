
require(["dojo/data/ItemFileWriteStore", "dojox/grid/DataGrid", "dijit/form/Button", "dijit/Dialog"]);

var orderAction = new OrderAction();
var orderId = "default";
var productList;
function OrderAction() {
    var order = this;

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


    this.setOrderIdByItem = function (id) {
        require(["dojo/dom", "dijit/registry", "dojo/dom-class", "dojo/json", "dojo/domReady!"],
                function (dom, registry, domClass) {
                    var OrderId = id;
                    orderId = OrderId;
                    console.log('onClick:' + orderId);
                });
    };

    this.dbclickOrderGrid = function (evt) {
        require(["dojo/_base/xhr", "dojo/dom", "dijit/registry", "dojo/domReady!"], function (xhr, dom, registry) {

            gridAction.showOrderUpdateContentPane();

        });
    };

    

    var CircularJSON = function (e, t) { function l(e, t, o) { var u = [], f = [e], l = [e], c = [o ? n : "[Circular]"], h = e, p = 1, d; return function (e, v) { return t && (v = t.call(this, e, v)), e !== "" && (h !== this && (d = p - a.call(f, this) - 1, p -= d, f.splice(p, f.length), u.splice(p - 1, u.length), h = this), typeof v == "object" && v ? (a.call(f, v) < 0 && f.push(h = v), p = f.length, d = a.call(l, v), d < 0 ? (d = l.push(v) - 1, o ? (u.push(("" + e).replace(s, r)), c[d] = n + u.join(n)) : c[d] = c[0]) : v = c[d]) : typeof v == "string" && o && (v = v.replace(r, i).replace(n, r))), v } } function c(e, t) { for (var r = 0, i = t.length; r < i; e = e[t[r++].replace(o, n)]); return e } function h(e) { return function (t, s) { var o = typeof s == "string"; return o && s.charAt(0) === n ? new f(s.slice(1)) : (t === "" && (s = v(s, s, {})), o && (s = s.replace(u, "$1" + n).replace(i, r)), e ? e.call(this, t, s) : s) } } function p(e, t, n) { for (var r = 0, i = t.length; r < i; r++) t[r] = v(e, t[r], n); return t } function d(e, t, n) { for (var r in t) t.hasOwnProperty(r) && (t[r] = v(e, t[r], n)); return t } function v(e, t, r) { return t instanceof Array ? p(e, t, r) : t instanceof f ? t.length ? r.hasOwnProperty(t) ? r[t] : r[t] = c(e, t.split(n)) : e : t instanceof Object ? d(e, t, r) : t } function m(t, n, r, i) { return e.stringify(t, l(t, n, !i), r) } function g(t, n) { return e.parse(t, h(n)) } var n = "~", r = "\\x" + ("0" + n.charCodeAt(0).toString(16)).slice(-2), i = "\\" + r, s = new t(r, "g"), o = new t(i, "g"), u = new t("(?:^|([^\\\\]))" + i), a = [].indexOf || function (e) { for (var t = this.length; t-- && this[t] !== e;); return t }, f = String; return { stringify: m, parse: g } }(JSON, RegExp);
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
                        if (jsonData!=false) {
                            xhr.post({
                                url: "/Order/InsertOrderDetail",
                                postData: CircularJSON.stringify({ id: jsonData, items: data.items }),
                                headers: { 'Content-Type': 'application/json' },
                                handleAs: "json",
                                load: function (jsonData) {
                                    console.log(jsonData);
                                    if (jsonData.State) {
                                        console.log(jsonData.State);
                                        gridAction.AddSearchProductRow(jsonData.Order.OrderId, jsonData.Order.CustName, jsonData.Order.O_Orderdate, jsonData.Order.O_ShippedDate);
                                        dom.byId("insertOrderForm").reset();
                                        
                                        InsertDialog.hide();
                                    }
                                    console.log(jsonData);
                                },
                                error: function (e) {
                                    console.log(e);
                                }
                            });
                        }
                    },
                    error: function (e) {
                        console.log(e);
                    }
                });
            }
        );
    }

    this.UpdateOrder = function () {
        require(["dojo/data/ItemFileWriteStore", "dojo/dom", "dojo/_base/xhr", "dojo/request", "dojo/dom-form", "dojox/json/ref", "dojo/ready", "dojo/json"],
            function (ItemFileWriteStore, dom, xhr, request, domForm, ref, ready) {
                //console.log(ref.toJson(data.items));
                xhr.post({
                    url: "/Order/UpdateOrder",
                    form: dom.byId("updateOrderForm"),
                    handleAs: "json",
                    load: function (jsonData) {
                        //console.log(updateData.items);
                        console.log(orderId);
                        console.log(updateData);
                        if (jsonData != false) {
                            console.log(updateData);
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
                        }
                    },
                    error: function (e) {
                        console.log(e);
                    }
                });
            }
        );
    }


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

}




//var object;
//orderAction.GetProductList(function (Response) { object = dojo.eval(Response) });
//console.log(orderAction.GetProductList(function (Response) { object = dojo.eval(Response) }));