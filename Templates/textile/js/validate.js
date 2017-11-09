
(function() {
    jQuery(document).ready(function() {
        setValidateForm();
    });

 
    this.setValidateForm = function(selector) {
        if (selector == null) {
            selector = jQuery(".validate-form");
        }
        return selector.each(function(i, elem) {
            return jQuery(elem).validate({
                errorElement: "div",
                errorClass: "help-block error",
                errorPlacement: function(e, t) {
                    return t.parents(".controls").append(e);
                },
                highlight: function(e) {
                    return jQuery(e).closest(".form-group").removeClass("error success").addClass("error");
                },
                success: function(e) {
                    return e.closest(".form-group").removeClass("error");
                }
            });
        });
    };

    
}).call(this);