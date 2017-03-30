
require(["dojo/data/ItemFileWriteStore", "dojox/grid/DataGrid"]);

var orderAction = new OrderAction();
var orderId = "default";


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
                            console.log(jsonData);
                            store = new ItemFileWriteStore({ data: jsonData });
                            
                            ready(function () {
                                orderStore.close();
                                console.log(store);
                                orderGrid.setStore(store);
                                console.log(orderGrid.store);
                                orderGrid._refresh();
                            });
                            
                        },
                        error: function (e) {
                            console.log(e);
                        }
                    });
                    


            }
        );

    }

    this.setOrderId = function (evt) {
        console.log('onClick:');
        require(["dojo/dom", "dijit/registry", "dojo/dom-class", "dojo/json", "dojo/domReady!"],
                function (dom, registry, domClass) {
                    var OrderId = orderGrid.getItem(evt.rowIndex).OrderId;
                    orderId = OrderId;

                    console.log('onClick:' + orderId);
                });
    };

    this.dbclickOrderGrid = function (evt) {
        require(["dojo/_base/xhr", "dojo/dom", "dijit/registry", "dojo/domReady!"], function (xhr, dom, registry) {

            console.log(orderId + '/');
            order.showOrderUpdateContentPane();

        });
    };

    this.showOrderUpdateContentPane = function () {
        require(["dojo/dom", "dojo/_base/xhr", "dijit/registry", "dojo/data/ItemFileWriteStore", "dojo/json", "dojo/domReady!"],
                function (dom, xhr, registry, ItemFileWriteStore) {
                    if (orderId == 'default') {
                        registry.byId("roomDialogContentPane").setContent('未選選項');
                        RoomDialog.show();
                    } else {
                        xhr.get({
                            url: "index.php?dir=room&action=ShowRoomUpdateContentPane&id=" + roomId,
                            load: function (jsonData) {
                                registry.byId("searchInformationContentPane").setContent(jsonData);
                            },
                            error: function (e) {
                                console.log(e);
                            }
                        });

                    }

                });
    };


}