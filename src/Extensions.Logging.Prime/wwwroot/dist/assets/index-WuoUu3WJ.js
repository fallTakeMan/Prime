import{B as $,aF as T,a2 as D,P as b,a3 as v,aG as K,aH as w,R as P,e as x,O as H,g as z,o as g,h as k,k as j,T as I,D as L,i as A,f as B,c as C,C as _,ac as R,X as Z,Y as N,aI as U,$ as M,L as S,a9 as V,ad as Y,a0 as E,a1 as q}from"./index-BFdu6KXE.js";import{d as G,R as X,s as J,O as m}from"./index-DKgpYIyM.js";var Q=$.extend({name:"focustrap-directive"}),W=T.extend({style:Q});function h(t){"@babel/helpers - typeof";return h=typeof Symbol=="function"&&typeof Symbol.iterator=="symbol"?function(e){return typeof e}:function(e){return e&&typeof Symbol=="function"&&e.constructor===Symbol&&e!==Symbol.prototype?"symbol":typeof e},h(t)}function F(t,e){var n=Object.keys(t);if(Object.getOwnPropertySymbols){var o=Object.getOwnPropertySymbols(t);e&&(o=o.filter(function(i){return Object.getOwnPropertyDescriptor(t,i).enumerable})),n.push.apply(n,o)}return n}function O(t){for(var e=1;e<arguments.length;e++){var n=arguments[e]!=null?arguments[e]:{};e%2?F(Object(n),!0).forEach(function(o){ee(t,o,n[o])}):Object.getOwnPropertyDescriptors?Object.defineProperties(t,Object.getOwnPropertyDescriptors(n)):F(Object(n)).forEach(function(o){Object.defineProperty(t,o,Object.getOwnPropertyDescriptor(n,o))})}return t}function ee(t,e,n){return(e=te(e))in t?Object.defineProperty(t,e,{value:n,enumerable:!0,configurable:!0,writable:!0}):t[e]=n,t}function te(t){var e=ne(t,"string");return h(e)=="symbol"?e:e+""}function ne(t,e){if(h(t)!="object"||!t)return t;var n=t[Symbol.toPrimitive];if(n!==void 0){var o=n.call(t,e);if(h(o)!="object")return o;throw new TypeError("@@toPrimitive must return a primitive value.")}return(e==="string"?String:Number)(t)}var oe=W.extend("focustrap",{mounted:function(e,n){var o=n.value||{},i=o.disabled;i||(this.createHiddenFocusableElements(e,n),this.bind(e,n),this.autoElementFocus(e,n)),e.setAttribute("data-pd-focustrap",!0),this.$el=e},updated:function(e,n){var o=n.value||{},i=o.disabled;i&&this.unbind(e)},unmounted:function(e){this.unbind(e)},methods:{getComputedSelector:function(e){return':not(.p-hidden-focusable):not([data-p-hidden-focusable="true"])'.concat(e??"")},bind:function(e,n){var o=this,i=n.value||{},r=i.onFocusIn,a=i.onFocusOut;e.$_pfocustrap_mutationobserver=new MutationObserver(function(s){s.forEach(function(u){if(u.type==="childList"&&!e.contains(document.activeElement)){var l=function(c){var d=w(c)?w(c,o.getComputedSelector(e.$_pfocustrap_focusableselector))?c:v(e,o.getComputedSelector(e.$_pfocustrap_focusableselector)):v(c);return P(d)?d:c.nextSibling&&l(c.nextSibling)};b(l(u.nextSibling))}})}),e.$_pfocustrap_mutationobserver.disconnect(),e.$_pfocustrap_mutationobserver.observe(e,{childList:!0}),e.$_pfocustrap_focusinlistener=function(s){return r&&r(s)},e.$_pfocustrap_focusoutlistener=function(s){return a&&a(s)},e.addEventListener("focusin",e.$_pfocustrap_focusinlistener),e.addEventListener("focusout",e.$_pfocustrap_focusoutlistener)},unbind:function(e){e.$_pfocustrap_mutationobserver&&e.$_pfocustrap_mutationobserver.disconnect(),e.$_pfocustrap_focusinlistener&&e.removeEventListener("focusin",e.$_pfocustrap_focusinlistener)&&(e.$_pfocustrap_focusinlistener=null),e.$_pfocustrap_focusoutlistener&&e.removeEventListener("focusout",e.$_pfocustrap_focusoutlistener)&&(e.$_pfocustrap_focusoutlistener=null)},autoFocus:function(e){this.autoElementFocus(this.$el,{value:O(O({},e),{},{autoFocus:!0})})},autoElementFocus:function(e,n){var o=n.value||{},i=o.autoFocusSelector,r=i===void 0?"":i,a=o.firstFocusableSelector,s=a===void 0?"":a,u=o.autoFocus,l=u===void 0?!1:u,p=v(e,"[autofocus]".concat(this.getComputedSelector(r)));l&&!p&&(p=v(e,this.getComputedSelector(s))),b(p)},onFirstHiddenElementFocus:function(e){var n,o=e.currentTarget,i=e.relatedTarget,r=i===o.$_pfocustrap_lasthiddenfocusableelement||!((n=this.$el)!==null&&n!==void 0&&n.contains(i))?v(o.parentElement,this.getComputedSelector(o.$_pfocustrap_focusableselector)):o.$_pfocustrap_lasthiddenfocusableelement;b(r)},onLastHiddenElementFocus:function(e){var n,o=e.currentTarget,i=e.relatedTarget,r=i===o.$_pfocustrap_firsthiddenfocusableelement||!((n=this.$el)!==null&&n!==void 0&&n.contains(i))?D(o.parentElement,this.getComputedSelector(o.$_pfocustrap_focusableselector)):o.$_pfocustrap_firsthiddenfocusableelement;b(r)},createHiddenFocusableElements:function(e,n){var o=this,i=n.value||{},r=i.tabIndex,a=r===void 0?0:r,s=i.firstFocusableSelector,u=s===void 0?"":s,l=i.lastFocusableSelector,p=l===void 0?"":l,c=function(y){return K("span",{class:"p-hidden-accessible p-hidden-focusable",tabIndex:a,role:"presentation","aria-hidden":!0,"data-p-hidden-accessible":!0,"data-p-hidden-focusable":!0,onFocus:y==null?void 0:y.bind(o)})},d=c(this.onFirstHiddenElementFocus),f=c(this.onLastHiddenElementFocus);d.$_pfocustrap_lasthiddenfocusableelement=f,d.$_pfocustrap_focusableselector=u,d.setAttribute("data-pc-section","firstfocusableelement"),f.$_pfocustrap_firsthiddenfocusableelement=d,f.$_pfocustrap_focusableselector=p,f.setAttribute("data-pc-section","lastfocusableelement"),e.prepend(d),e.append(f)}}}),re=({dt:t})=>`
.p-popover {
    margin-block-start: ${t("popover.gutter")};
    background: ${t("popover.background")};
    color: ${t("popover.color")};
    border: 1px solid ${t("popover.border.color")};
    border-radius: ${t("popover.border.radius")};
    box-shadow: ${t("popover.shadow")};
}

.p-popover-content {
    padding: ${t("popover.content.padding")};
}

.p-popover-flipped {
    margin-block-start: calc(${t("popover.gutter")} * -1);
    margin-block-end: ${t("popover.gutter")};
}

.p-popover-enter-from {
    opacity: 0;
    transform: scaleY(0.8);
}

.p-popover-leave-to {
    opacity: 0;
}

.p-popover-enter-active {
    transition: transform 0.12s cubic-bezier(0, 0, 0.2, 1), opacity 0.12s cubic-bezier(0, 0, 0.2, 1);
}

.p-popover-leave-active {
    transition: opacity 0.1s linear;
}

.p-popover:after,
.p-popover:before {
    bottom: 100%;
    left: calc(${t("popover.arrow.offset")} + ${t("popover.arrow.left")});
    content: " ";
    height: 0;
    width: 0;
    position: absolute;
    pointer-events: none;
}

.p-popover:after {
    border-width: calc(${t("popover.gutter")} - 2px);
    margin-left: calc(-1 * (${t("popover.gutter")} - 2px));
    border-style: solid;
    border-color: transparent;
    border-bottom-color: ${t("popover.background")};
}

.p-popover:before {
    border-width: ${t("popover.gutter")};
    margin-left: calc(-1 * ${t("popover.gutter")});
    border-style: solid;
    border-color: transparent;
    border-bottom-color: ${t("popover.border.color")};
}

.p-popover-flipped:after,
.p-popover-flipped:before {
    bottom: auto;
    top: 100%;
}

.p-popover.p-popover-flipped:after {
    border-bottom-color: transparent;
    border-top-color: ${t("popover.background")};
}

.p-popover.p-popover-flipped:before {
    border-bottom-color: transparent;
    border-top-color: ${t("popover.border.color")};
}
`,ie={root:"p-popover p-component",content:"p-popover-content"},se=$.extend({name:"popover",style:re,classes:ie}),ae={name:"BasePopover",extends:J,props:{dismissable:{type:Boolean,default:!0},appendTo:{type:[String,Object],default:"body"},baseZIndex:{type:Number,default:0},autoZIndex:{type:Boolean,default:!0},breakpoints:{type:Object,default:null},closeOnEscape:{type:Boolean,default:!0}},style:se,provide:function(){return{$pcPopover:this,$parentInstance:this}}},le={name:"Popover",extends:ae,inheritAttrs:!1,emits:["show","hide"],data:function(){return{visible:!1}},watch:{dismissable:{immediate:!0,handler:function(e){e?this.bindOutsideClickListener():this.unbindOutsideClickListener()}}},selfClick:!1,target:null,eventTarget:null,outsideClickListener:null,scrollHandler:null,resizeListener:null,container:null,styleElement:null,overlayEventListener:null,documentKeydownListener:null,beforeUnmount:function(){this.dismissable&&this.unbindOutsideClickListener(),this.scrollHandler&&(this.scrollHandler.destroy(),this.scrollHandler=null),this.destroyStyle(),this.unbindResizeListener(),this.target=null,this.container&&this.autoZIndex&&E.clear(this.container),this.overlayEventListener&&(m.off("overlay-click",this.overlayEventListener),this.overlayEventListener=null),this.container=null},mounted:function(){this.breakpoints&&this.createStyle()},methods:{toggle:function(e,n){this.visible?this.hide():this.show(e,n)},show:function(e,n){this.visible=!0,this.eventTarget=e.currentTarget,this.target=n||e.currentTarget},hide:function(){this.visible=!1},onContentClick:function(){this.selfClick=!0},onEnter:function(e){var n=this;q(e,{position:"absolute",top:"0",left:"0"}),this.alignOverlay(),this.dismissable&&this.bindOutsideClickListener(),this.bindScrollListener(),this.bindResizeListener(),this.autoZIndex&&E.set("overlay",e,this.baseZIndex+this.$primevue.config.zIndex.overlay),this.overlayEventListener=function(o){n.container.contains(o.target)&&(n.selfClick=!0)},this.focus(),m.on("overlay-click",this.overlayEventListener),this.$emit("show"),this.closeOnEscape&&this.bindDocumentKeyDownListener()},onLeave:function(){this.unbindOutsideClickListener(),this.unbindScrollListener(),this.unbindResizeListener(),this.unbindDocumentKeyDownListener(),m.off("overlay-click",this.overlayEventListener),this.overlayEventListener=null,this.$emit("hide")},onAfterLeave:function(e){this.autoZIndex&&E.clear(e)},alignOverlay:function(){M(this.container,this.target,!1);var e=S(this.container),n=S(this.target),o=0;e.left<n.left&&(o=n.left-e.left),this.container.style.setProperty(V("popover.arrow.left").name,"".concat(o,"px")),e.top<n.top&&(this.container.setAttribute("data-p-popover-flipped","true"),!this.isUnstyled&&Y(this.container,"p-popover-flipped"))},onContentKeydown:function(e){e.code==="Escape"&&this.closeOnEscape&&(this.hide(),b(this.target))},onButtonKeydown:function(e){switch(e.code){case"ArrowDown":case"ArrowUp":case"ArrowLeft":case"ArrowRight":e.preventDefault()}},focus:function(){var e=this.container.querySelector("[autofocus]");e&&e.focus()},onKeyDown:function(e){e.code==="Escape"&&this.closeOnEscape&&(this.visible=!1)},bindDocumentKeyDownListener:function(){this.documentKeydownListener||(this.documentKeydownListener=this.onKeyDown.bind(this),window.document.addEventListener("keydown",this.documentKeydownListener))},unbindDocumentKeyDownListener:function(){this.documentKeydownListener&&(window.document.removeEventListener("keydown",this.documentKeydownListener),this.documentKeydownListener=null)},bindOutsideClickListener:function(){var e=this;!this.outsideClickListener&&U()&&(this.outsideClickListener=function(n){e.visible&&!e.selfClick&&!e.isTargetClicked(n)&&(e.visible=!1),e.selfClick=!1},document.addEventListener("click",this.outsideClickListener,!0))},unbindOutsideClickListener:function(){this.outsideClickListener&&(document.removeEventListener("click",this.outsideClickListener,!0),this.outsideClickListener=null,this.selfClick=!1)},bindScrollListener:function(){var e=this;this.scrollHandler||(this.scrollHandler=new N(this.target,function(){e.visible&&(e.visible=!1)})),this.scrollHandler.bindScrollListener()},unbindScrollListener:function(){this.scrollHandler&&this.scrollHandler.unbindScrollListener()},bindResizeListener:function(){var e=this;this.resizeListener||(this.resizeListener=function(){e.visible&&!Z()&&(e.visible=!1)},window.addEventListener("resize",this.resizeListener))},unbindResizeListener:function(){this.resizeListener&&(window.removeEventListener("resize",this.resizeListener),this.resizeListener=null)},isTargetClicked:function(e){return this.eventTarget&&(this.eventTarget===e.target||this.eventTarget.contains(e.target))},containerRef:function(e){this.container=e},createStyle:function(){if(!this.styleElement&&!this.isUnstyled){var e;this.styleElement=document.createElement("style"),this.styleElement.type="text/css",R(this.styleElement,"nonce",(e=this.$primevue)===null||e===void 0||(e=e.config)===null||e===void 0||(e=e.csp)===null||e===void 0?void 0:e.nonce),document.head.appendChild(this.styleElement);var n="";for(var o in this.breakpoints)n+=`
                        @media screen and (max-width: `.concat(o,`) {
                            .p-popover[`).concat(this.$attrSelector,`] {
                                width: `).concat(this.breakpoints[o],` !important;
                            }
                        }
                    `);this.styleElement.innerHTML=n}},destroyStyle:function(){this.styleElement&&(document.head.removeChild(this.styleElement),this.styleElement=null)},onOverlayClick:function(e){m.emit("overlay-click",{originalEvent:e,target:this.target})}},directives:{focustrap:oe,ripple:X},components:{Portal:G}},ce=["aria-modal"];function ue(t,e,n,o,i,r){var a=x("Portal"),s=H("focustrap");return g(),z(a,{appendTo:t.appendTo},{default:k(function(){return[j(I,L({name:"p-popover",onEnter:r.onEnter,onLeave:r.onLeave,onAfterLeave:r.onAfterLeave},t.ptm("transition")),{default:k(function(){return[i.visible?A((g(),C("div",L({key:0,ref:r.containerRef,role:"dialog","aria-modal":i.visible,onClick:e[3]||(e[3]=function(){return r.onOverlayClick&&r.onOverlayClick.apply(r,arguments)}),class:t.cx("root")},t.ptmi("root")),[t.$slots.container?_(t.$slots,"container",{key:0,closeCallback:r.hide,keydownCallback:function(l){return r.onButtonKeydown(l)}}):(g(),C("div",L({key:1,class:t.cx("content"),onClick:e[0]||(e[0]=function(){return r.onContentClick&&r.onContentClick.apply(r,arguments)}),onMousedown:e[1]||(e[1]=function(){return r.onContentClick&&r.onContentClick.apply(r,arguments)}),onKeydown:e[2]||(e[2]=function(){return r.onContentKeydown&&r.onContentKeydown.apply(r,arguments)})},t.ptm("content")),[_(t.$slots,"default")],16))],16,ce)),[[s]]):B("",!0)]}),_:3},16,["onEnter","onLeave","onAfterLeave"])]}),_:3},8,["appendTo"])}le.render=ue;export{oe as F,le as s};
