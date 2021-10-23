"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.ProductService = void 0;
var core_1 = require("@angular/core");
var operators_1 = require("rxjs/operators");
var ProductService = /** @class */ (function () {
    function ProductService(http, _sanitizer) {
        this.http = http;
        this._sanitizer = _sanitizer;
        this.productList = [];
        this.resuest = {};
        this.images = [];
    }
    ProductService.prototype.getProductImages = function (baseuri) {
        this.resuest = {};
        return this.http.put(baseuri + 'ProductsDetails/GetProducImages/' + 0, this.resuest)
            .pipe(operators_1.tap(// Log the result or error
        function (// Log the result or error
        data) {
            data;
        }, function (error) { return console.log(error); }));
    };
    ProductService.prototype.setProductList = function (completeProductList) {
        this.productList = completeProductList;
    };
    ProductService.prototype.returnproductWithImageList = function (imageDataResult) {
        if (this.productList) {
            var productsList_1 = this.productList;
            var _loop_1 = function (i) {
                var result = imageDataResult.filter(function (product) {
                    return product.productId === productsList_1[i].productId;
                });
                this_1.images = [];
                if (result && result.length > 0) {
                    for (var j = 0; j < result.length; j++) {
                        this_1.images.push(this_1._sanitizer.bypassSecurityTrustResourceUrl('data:image/jpg;base64,'
                            + result[j].image));
                    }
                    this_1.productList[i]["images"] = this_1.images;
                }
            };
            var this_1 = this;
            for (var i = 0; i < this.productList.length; i++) {
                _loop_1(i);
            }
        }
        return this.productList;
    };
    ProductService = __decorate([
        core_1.Injectable({
            providedIn: 'root'
        })
    ], ProductService);
    return ProductService;
}());
exports.ProductService = ProductService;
//# sourceMappingURL=product.service.js.map