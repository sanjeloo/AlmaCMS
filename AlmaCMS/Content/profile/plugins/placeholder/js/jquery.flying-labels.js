/*
        jQuery flying-labels plugin.
        By Vladislav Fedotov.
        Released at 22.09.2017 - v 1.0.0
*/

;(function($){
    var methods = {
        show: function(e){
            var dataFor = $(e.target).attr('name');
            var $placeholder = $("."+config.placeholderClass+"[data-for='"+dataFor+"']");
            var $field = $("[name='"+dataFor+"']");
            if($field.val()!=""){
                if($placeholder.first().length==0){
                    $("<div/>", {
                        "class": config.placeholderClass,
                        "data-for": dataFor,
                        text: $field.attr('placeholder')
                    }).insertAfter($field).css('animation', config.showAnimation+' forwards '+config.animationTime+'ms').show();
                }else $placeholder.first().css('animation', config.showAnimation+' forwards '+config.animationTime+'ms').show();
            }  
        },  
        hide: function(e){
            var dataFor = $(e.target).attr('name');
            var $placeholder = $("."+config.placeholderClass+"[data-for='"+dataFor+"']");
            var $field = $("[name='"+dataFor+"']");
            if($field.val()==""){
                $placeholder.first().css('animation', config.hideAnimation+' forwards '+config.animationTime/2+'ms');
            }
            
        },
        init: function(obj){
            if (obj[0].tagName === "INPUT" && obj.attr('type') != "password" && obj.attr('type') != "text" && obj.attr('type') != "email" && obj.attr('type') != "number") return;
            if(obj[0].tagName!=="INPUT" && obj[0].tagName!=="TEXTAREA") return;
            if(typeof obj.attr('placeholder')==="undefined") return;
            if(!obj.parent().hasClass(config.wrapperClass)) obj.wrap("<div class='"+config.wrapperClass+"'/>");
            if(obj.val().trim()!=""){
                $("<div/>", {
                    "class": config.placeholderClass,
                    "data-for": obj.attr('name'),
                    text: obj.attr('placeholder')
                }).insertAfter(obj).fadeIn(config.animationTime);
            }
            obj.on("keyup.fly", methods.show);
            obj.on("change.fly keyup.fly keydown.fly", methods.hide);
        },
        destroy: function(obj){
            obj.off("keyup.fly change.fly keydown.fly");
            $("."+config.placeholderClass+"[data-for='"+obj.attr('name')+"']").remove();
        }
    }
    var config, defaults = {
        'wrapperClass': 'it-wrapper',
        'placeholderClass': 'placeholder',
        'animationTime': '200',
        'showAnimation': 'slideInUp',
        'hideAnimation': 'zoomOut'
    };
    $.fn.fly = function(options){
        if(options!=="destroy") config = $.extend(defaults, options);
        return this.each(function(){    
            if(options!=="destroy") methods.init($(this));
            else methods.destroy($(this));
        });
    }
})(jQuery);