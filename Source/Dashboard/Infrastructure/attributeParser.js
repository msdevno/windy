import * as ko from "ko";

export class attributeParser
{
    parse(value) {
        var items = ko.expressionRewriting.parseObjectLiteral(value);
        var target = {};

        items.forEach(function (item) {
            var value = item.value.trim();

            value = value.split("'").join("");
            value = value.split('"').join("");
            target[item.key.trim()] = value;
        });
        return target;
    }
}