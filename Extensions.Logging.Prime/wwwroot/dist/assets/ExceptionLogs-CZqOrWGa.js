import{_ as se,I as le,p as d,q as te,c as v,a as s,h as l,g as a,f as oe,o as p,w as k,t as i,J as ae,F as ie,i as ne,b as de,l as f,e as N,K as re}from"./index-CV1zKm2Z.js";import{b as ue}from"./index-MBs-NB0l.js";import{s as ce,a as pe,L as j,b as ve,c as me}from"./LogService-C2bHCBo1.js";import{s as fe,a as ge,b as be,c as _e,d as xe,e as ye}from"./index-Nqr7qCb3.js";import{s as he}from"./index-C4BEgG6t.js";import"./index-BI5Bdk_9.js";import"./request-Bv6X9YoH.js";import"./index-CzjqAukc.js";const ke={class:"card"},Le={class:"flex justify-between"},Te={class:"flex justify-between gap-4"},we={class:"flex justify-between gap-4"},Ce=["onClick"],Se={class:"limited-text"},Pe=["onClick"],Ee={class:"limited-text"},Ie=["onClick"],Me={class:"limited-text"},Ne=["onClick"],De={class:"limited-text"},Re={class:"mb-2"},$e={class:"mb-2"},qe={class:"mb-2"},Ve={class:"mb-2"},Be={class:"mb-2"},He={class:"mb-2"},ze={class:"mb-2"},Ue={class:"mb-2"},Ae={class:"mb-2"},je={class:"mb-2"},Fe={class:"mb-2"},Ge={class:"mb-2"},Ke={key:0,class:"mb-2"},Oe={class:"mb-2"},Qe={class:"mb-2"},Je={class:"mb-2"},Ye={class:"m-0"},We={class:"m-0"},Xe={class:"m-0"},Ze={class:"flex items-center gap-4"},es={key:0},ss={key:0,class:"rounded flex flex-col"},ls={class:"flex justify-center rounded"},ts={class:"relative mx-auto"},os={__name:"ExceptionLogs",setup(as){const L=le(),D=d(),b=d(null),m=d(),o=d(),x=d(null),_=d(!1),T=d(!1),w=d(),R=d(""),$=d(),q=d(1),C=d(20),V=d(0),S=d([{field:"requestHeaders",header:"RequestHeaders"},{field:"requestBody",header:"RequestBody"},{field:"ipAddress",header:"IpAddress"},{field:"connectionId",header:"ConnectionId"},{field:"traceId",header:"TraceId"},{field:"eventId",header:"EventId"},{field:"eventName",header:"EventName"},{field:"logLevel",header:"LogLevel"},{field:"logger",header:"Logger"}]),P=d(S.value),F=n=>{P.value=S.value.filter(e=>n.includes(e))},G=()=>{_.value=!0},K=()=>{D.value.exportCSV()},O=n=>{o.value=n,T.value=!0},y=(n,e,r)=>{var g;w.value.hide(),((g=o.value)==null?void 0:g.id)===e.id&&$.value===r?o.value=null:($.value=r,R.value=e[r],o.value=e,re(()=>{w.value.show(n)}))},E=()=>{x.value=!0,j.getExceptionLogs(q.value,C.value).then(n=>{b.value=n.data,V.value=n.paging.totalCount,x.value=!1,b.value.forEach(e=>e.logTime=new Date(e.logTime))}).catch(n=>{x.value=!1,L.add({severity:"error",summary:"loading data failed",detail:n.message,life:6e3})})},Q=n=>{q.value=n.page+1,C.value=n.rows,E()},J=()=>{var n=m.value.map(e=>e.id);j.deleteExceptionLogs(n).then(()=>{b.value=b.value.filter(e=>!m.value.includes(e)),_.value=!1,m.value=null,L.add({severity:"success",summary:"Successful",detail:"Logs Deleted",life:3e3})}).catch(e=>{var r,g;L.add({severity:"error",summary:"Failed",detail:e.message+`\r
`+(((g=(r=e.response)==null?void 0:r.data)==null?void 0:g.message)??""),life:6e3})})},B=n=>n.toLocaleString("zh-CN"),H=n=>{switch(n){case"GET":return"info";case"POST":return"secondary";case"DELETE":return"danger"}};return te(()=>{E()}),(n,e)=>{const r=he,g=fe,u=ve,z=me,Y=ce,U=oe("slan"),I=_e,W=be,M=ye,X=xe,Z=ge,A=pe,ee=ue,h=ae;return p(),v("div",ke,[e[28]||(e[28]=s("div",{class:"font-semibold text-xl mb-4"},"Exception Logs",-1)),l(Y,{ref_key:"dt",ref:D,selection:m.value,"onUpdate:selection":e[1]||(e[1]=t=>m.value=t),value:b.value,loading:x.value,paginator:!0,rows:C.value,rowsPerPageOptions:[10,20,50,100,200,500],totalRecords:V.value,lazy:!0,rowHover:!0,currentPageReportTemplate:"Showing {first} to {last} of {totalRecords} logs",paginatorTemplate:"FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown",dataKey:"id",resizableColumns:"",columnResizeMode:"fit",showGridlines:"",scrollable:"",scrollHeight:"560px",onPage:e[2]||(e[2]=t=>Q(t))},{header:a(()=>[s("div",Le,[s("span",Te,[l(r,{label:"Refresh",icon:"pi pi-refresh",onClick:E,outlined:""}),l(r,{label:"Delete",icon:"pi pi-trash",severity:"danger",outlined:"",onClick:G,disabled:!m.value||!m.value.length},null,8,["disabled"]),l(r,{label:"Export",icon:"pi pi-upload",severity:"secondary",onClick:e[0]||(e[0]=t=>K(t))})]),s("span",we,[l(g,{modelValue:P.value,options:S.value,maxSelectedLabels:5,optionLabel:"header","onUpdate:modelValue":F,display:"chip",placeholder:"Select Columns"},null,8,["modelValue","options"])])])]),empty:a(()=>e[6]||(e[6]=[f(" No data found. ")])),loading:a(()=>e[7]||(e[7]=[f(" Loading exception logs. Please wait. ")])),default:a(()=>[l(u,{selectionMode:"multiple",headerStyle:"width: 3rem"}),l(u,{exportable:!1},{body:a(({data:t})=>[l(r,{icon:"pi pi-search",onClick:c=>O(t),severity:"secondary",rounded:""},null,8,["onClick"])]),_:1}),l(u,{field:"method",header:"Method",style:{"max-width":"12rem"}},{body:a(({data:t})=>[l(z,{value:t.method,severity:H(t.method)},null,8,["value","severity"])]),_:1}),l(u,{field:"host",header:"Host"}),l(u,{field:"path",header:"Path"}),l(u,{field:"queryString",header:"QueryString"}),l(u,{field:"message",header:"Message"},{body:a(({data:t})=>[k((p(),v("div",{class:"flex items-center gap-2",onClick:c=>y(c,t,"message")},[s("span",Se,i(t.message),1)],8,Ce)),[[h,"click to view detail",void 0,{top:!0}]])]),_:1}),l(u,{field:"exceptionSource",header:"ExceptionSource"},{body:a(({data:t})=>[k((p(),v("div",{class:"flex items-center gap-2",onClick:c=>y(c,t,"exceptionSource")},[s("span",Ee,i(t.exceptionSource),1)],8,Pe)),[[h,"click to view detail",void 0,{top:!0}]])]),_:1}),l(u,{field:"exceptionMessage",header:"ExceptionMessage"},{body:a(({data:t})=>[k((p(),v("div",{class:"flex items-center gap-2",onClick:c=>y(c,t,"exceptionMessage")},[s("span",Me,i(t.exceptionMessage),1)],8,Ie)),[[h,"click to view detail",void 0,{top:!0}]])]),_:1}),l(u,{field:"exceptionStackTrace",header:"ExceptionStackTrace"},{body:a(({data:t})=>[k((p(),v("div",{class:"flex items-center gap-2",onClick:c=>y(c,t,"exceptionStackTrace")},[s("span",De,i(t.exceptionStackTrace),1)],8,Ne)),[[h,"click to view detail",void 0,{top:!0}]])]),_:1}),(p(!0),v(ie,null,ne(P.value,(t,c)=>(p(),de(u,{field:t.field,header:t.header,key:t.field+"_"+c,style:{"max-width":"20rem"}},null,8,["field","header"]))),128)),l(u,{field:"logTime",header:"LogTime"},{body:a(({data:t})=>[f(i(B(t.logTime)),1)]),_:1}),l(u,{field:"userName",header:"UserName"})]),_:1},8,["selection","value","loading","rows","totalRecords"]),l(A,{visible:T.value,"onUpdate:visible":e[3]||(e[3]=t=>T.value=t),modal:"",maximizable:"",header:"Log Detail",style:{width:"65rem"},breakpoints:{"1199px":"75vw","575px":"90vw"}},{default:a(()=>[s("div",null,[s("div",Re,[e[8]||(e[8]=s("label",{class:"font-bold"},"[Method]: ",-1)),l(z,{value:o.value.method,severity:H(o.value.method)},null,8,["value","severity"])]),s("div",$e,[e[9]||(e[9]=s("label",{class:"font-bold"},"[Host]: ",-1)),s("span",null,i(o.value.host),1)]),s("div",qe,[e[10]||(e[10]=s("label",{class:"font-bold"},"[Path]: ",-1)),s("span",null,i(o.value.path),1)]),s("div",Ve,[e[11]||(e[11]=s("label",{class:"font-bold"},"[QueryString]: ",-1)),s("span",null,i(o.value.queryString),1)]),s("div",Be,[e[12]||(e[12]=s("label",{class:"font-bold"},"[IpAddress]: ",-1)),s("span",null,i(o.value.ipAddress),1)]),s("div",He,[e[13]||(e[13]=s("label",{class:"font-bold"},"[ConnectionId]: ",-1)),s("span",null,i(o.value.connectionId),1)]),s("div",ze,[e[14]||(e[14]=s("label",{class:"font-bold"},"[TraceId]: ",-1)),s("span",null,i(o.value.traceId),1)]),s("div",Ue,[e[15]||(e[15]=s("label",{class:"font-bold"},"[EventId]: ",-1)),s("span",null,i(o.value.eventId),1)]),s("div",Ae,[e[16]||(e[16]=s("label",{class:"font-bold"},"[EventName]: ",-1)),s("span",null,i(o.value.eventName),1)]),s("div",je,[e[17]||(e[17]=s("label",{class:"font-bold"},"[LogLevel]: ",-1)),s("span",null,i(o.value.logLevel),1)]),s("div",Fe,[e[18]||(e[18]=s("label",{class:"font-bold"},"[Logger]: ",-1)),s("span",null,i(o.value.logger),1)]),s("div",Ge,[e[19]||(e[19]=s("label",{class:"font-bold"},"[LogTime]: ",-1)),s("span",null,i(B(o.value.logTime)),1)]),o.value.userName?(p(),v("div",Ke,[e[20]||(e[20]=s("label",{class:"font-bold"},"[UserName]: ",-1)),s("span",null,i(o.value.userName),1)])):N("",!0),s("div",Oe,[e[21]||(e[21]=s("label",{class:"font-bold"},"[Message]: ",-1)),s("span",null,i(o.value.message),1)]),s("div",Qe,[e[22]||(e[22]=s("label",{class:"font-bold"},"[ExceptionSource]: ",-1)),l(U,null,{default:a(()=>[f(i(o.value.exceptionSource),1)]),_:1})]),s("div",Je,[e[23]||(e[23]=s("label",{class:"font-bold"},"[ExceptionMessage]: ",-1)),l(U,null,{default:a(()=>[f(i(o.value.exceptionMessage),1)]),_:1})]),l(Z,{value:"0"},{default:a(()=>[l(W,null,{default:a(()=>[l(I,{value:"0"},{default:a(()=>e[24]||(e[24]=[f("ExceptionStackTrace")])),_:1}),l(I,{value:"1"},{default:a(()=>e[25]||(e[25]=[f("RequestBody")])),_:1}),l(I,{value:"2"},{default:a(()=>e[26]||(e[26]=[f("RequestHeaders")])),_:1})]),_:1}),l(X,null,{default:a(()=>[l(M,{value:"0"},{default:a(()=>[s("p",Ye,i(o.value.exceptionStackTrace),1)]),_:1}),l(M,{value:"1"},{default:a(()=>[s("p",We,i(o.value.requestBody),1)]),_:1}),l(M,{value:"2"},{default:a(()=>[s("p",Xe,i(o.value.requestHeaders),1)]),_:1})]),_:1})]),_:1})])]),_:1},8,["visible"]),l(A,{visible:_.value,"onUpdate:visible":e[5]||(e[5]=t=>_.value=t),modal:"",header:"Confirm",style:{width:"450px"}},{footer:a(()=>[l(r,{label:"No",icon:"pi pi-times",text:"",onClick:e[4]||(e[4]=t=>_.value=!1)}),l(r,{label:"Yes",icon:"pi pi-check",text:"",onClick:J})]),default:a(()=>[s("div",Ze,[e[27]||(e[27]=s("i",{class:"pi pi-exclamation-triangle !text-3xl"},null,-1)),m.value?(p(),v("span",es,"Are you sure you want to delete the selected logs?")):N("",!0)])]),_:1},8,["visible"]),l(ee,{ref_key:"msgPop",ref:w},{default:a(()=>[o.value?(p(),v("div",ss,[s("div",ls,[s("div",ts,[s("span",null,i(R.value),1)])])])):N("",!0)]),_:1},512)])}}},ms=se(os,[["__scopeId","data-v-2e0df906"]]);export{ms as default};
