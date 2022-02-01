/**
 * @license Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
    // config.uiColor = '#AADC6E';

    var roxyFileman = '/Scripts/RoxyFileManager/index.html?integration=ckeditor';
    config.filebrowserBrowseUrl = roxyFileman;
    config.filebrowserImageBrowseUrl = roxyFileman + '&type=image';
    config.removeDialogTabs = 'link:upload;image:upload';
    config.toolbar = 'MyBasic';
    config.allowedContent = true;


    config.contentsCss = '../content/css/persian.css';
    //the next line add the new font to the combobox in CKEditor
    config.font_names = 'IranSans FA/IranSans;' + config.font_names;


    //config.toolbar = [
     

    // '/',
    // { name: 'basicstyles', groups: ['basicstyles', 'cleanup'], items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'CopyFormatting'] },
    // { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi'], items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-', 'BidiLtr', 'BidiRtl'] },
    // { name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
    // { name: 'styles', items: ['Font', 'FontSize','Source'] },
    // { name: 'insert', items: ['Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak', 'Iframe', 'Maximize', 'ShowBlocks'] },
    // '/',
    //];

    //config.toolbarGroups = [
    //{ name: 'basicstyles' },
    //{ name: 'clipboard' },
    //{ name: 'undo' },
    //{ name: 'links' },
    //{ name: 'insert' },
    //{ name: 'paragraph', groups: ['align'] },
    //{ name: 'mode' },
    //{ name: 'list' },
    //{ name: 'tools' }
    //, { name: 'indent' }
   
    //]
};
