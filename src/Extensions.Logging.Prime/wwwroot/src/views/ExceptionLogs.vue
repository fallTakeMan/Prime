<script setup>
import { nextTick, onBeforeMount, ref } from "vue";
import { useToast } from "primevue/usetoast";
import { LogService } from "@/service/LogService";

const toast = useToast();
const dt = ref();
const tbl = ref(null);
const selectedTbl = ref();
const selectedLog = ref();
const loading = ref(null);
const deleteDialogVisible = ref(false);
const detailVisible = ref(false);
const msgPop = ref();
const dynamicPopMsg = ref("");
const selectedColumn = ref();
const pageIndex = ref(1);
const pageSize = ref(20);
const totalRecords = ref(0);
const columns = ref([
  { field: "requestHeaders", header: "RequestHeaders" },
  { field: "requestBody", header: "RequestBody" },
  { field: "ipAddress", header: "IpAddress" },
  { field: "connectionId", header: "ConnectionId" },
  { field: "traceId", header: "TraceId" },
  { field: "eventId", header: "EventId" },
  { field: "eventName", header: "EventName" },
  { field: "logLevel", header: "LogLevel" },
  { field: "logger", header: "Logger" },
]);
const selectedColumns = ref(columns.value);
const onToggle = (val) => {
  selectedColumns.value = columns.value.filter((col) => val.includes(col));
};
const showDeleteDialog = () => {
  deleteDialogVisible.value = true;
};
const exportCSV = () => {
  dt.value.exportCSV();
};
const viewLog = (data) => {
  selectedLog.value = data;
  detailVisible.value = true;
};
const displayPopMsg = (event, log, columnName) => {
  msgPop.value.hide();
  if (selectedLog.value?.id === log.id && selectedColumn.value === columnName) {
    selectedLog.value = null;
  } else {
    selectedColumn.value = columnName;
    dynamicPopMsg.value = log[columnName];
    selectedLog.value = log;
    nextTick(() => {
      msgPop.value.show(event);
    });
  }
};
const getExceptionLogs = () => {
  loading.value = true;
  LogService.getExceptionLogs(pageIndex.value, pageSize.value)
    .then((resp) => {
      tbl.value = resp.data;
      totalRecords.value = resp.paging.totalCount;
      loading.value = false;
      tbl.value.forEach((v) => (v.logTime = new Date(v.logTime)));
    })
    .catch((error) => {
      loading.value = false;
      toast.add({
        severity: "error",
        summary: "loading exception logs failed",
        detail: error.message,
        life: 6000,
      });
    });
};
const onPageChange = (event) => {
  pageIndex.value = event.page + 1;
  pageSize.value = event.rows;
  getExceptionLogs();
};
const confirmDeleteSelected = () => {
  var ids = selectedTbl.value.map((x) => x.id);
  LogService.deleteExceptionLogs(ids)
    .then(() => {
      tbl.value = tbl.value.filter((val) => !selectedTbl.value.includes(val));
      deleteDialogVisible.value = false;
      selectedTbl.value = null;
      toast.add({
        severity: "success",
        summary: "Successful",
        detail: "Logs Deleted",
        life: 3000,
      });
    })
    .catch((error) => {
      toast.add({
        severity: "error",
        summary: "delete exception logs failed",
        detail: error.message + "\r\n" + (error.response?.data?.message ?? ""),
        life: 6000,
      });
    });
};
const formateTime = (value) => {
  return value.toLocaleString("zh-CN");
};
const getSeverity = (method) => {
  switch (method) {
    case "GET":
      return "info";
    case "POST":
      return "success";
    case "PUT":
      return "warn";
    case "DELETE":
      return "danger";
  }
};
onBeforeMount(() => {
  getExceptionLogs();
});
</script>

<template>
  <div class="card">
    <div class="font-semibold text-xl mb-4">Exception Logs</div>
    <DataTable
      ref="dt"
      v-model:selection="selectedTbl"
      :value="tbl"
      :loading="loading"
      :paginator="true"
      :rows="pageSize"
      :rowsPerPageOptions="[10, 20, 50, 100, 200, 500]"
      :totalRecords="totalRecords"
      :lazy="true"
      :rowHover="true"
      currentPageReportTemplate="Showing {first} to {last} of {totalRecords} logs"
      paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
      dataKey="id"
      resizableColumns
      columnResizeMode="fit"
      showGridlines
      scrollable
      scrollHeight="560px"
      @page="onPageChange($event)"
    >
      <template #header>
        <div class="flex justify-between">
          <span class="flex justify-between gap-4">
            <Button
              label="Refresh"
              icon="pi pi-refresh"
              @click="getExceptionLogs"
              outlined
            />
            <Button
              label="Delete"
              icon="pi pi-trash"
              severity="danger"
              outlined
              @click="showDeleteDialog"
              :disabled="!selectedTbl || !selectedTbl.length"
            />
            <Button
              label="Export"
              icon="pi pi-upload"
              severity="secondary"
              @click="exportCSV($event)"
            />
          </span>
          <span class="flex justify-between gap-4">
            <MultiSelect
              :modelValue="selectedColumns"
              :options="columns"
              :maxSelectedLabels="5"
              optionLabel="header"
              @update:modelValue="onToggle"
              display="chip"
              placeholder="Select Columns"
            />
          </span>
        </div>
      </template>
      <template #empty> No data found. </template>
      <template #loading> Loading exception logs. Please wait. </template>
      <Column selectionMode="multiple" headerStyle="width: 3rem"></Column>
      <Column :exportable="false">
        <template #body="{ data }">
          <Button
            icon="pi pi-search"
            @click="viewLog(data)"
            severity="secondary"
            rounded
          ></Button>
        </template>
      </Column>
      <Column field="method" header="Method" style="max-width: 12rem">
        <template #body="{ data }">
          <Tag :value="data.method" :severity="getSeverity(data.method)"></Tag>
        </template>
      </Column>
      <Column field="host" header="Host"></Column>
      <Column field="path" header="Path"></Column>
      <Column field="queryString" header="QueryString"></Column>
      <Column field="message" header="Message">
        <template #body="{ data }">
          <div
            class="flex items-center gap-2"
            @click="displayPopMsg($event, data, 'message')"
            v-tooltip.top="'click to view detail'"
          >
            <span class="limited-text"> {{ data.message }}</span>
          </div>
        </template>
      </Column>
      <Column field="exceptionSource" header="ExceptionSource">
        <template #body="{ data }">
          <div
            class="flex items-center gap-2"
            @click="displayPopMsg($event, data, 'exceptionSource')"
            v-tooltip.top="'click to view detail'"
          >
            <span class="limited-text"> {{ data.exceptionSource }}</span>
          </div>
        </template>
      </Column>
      <Column field="exceptionMessage" header="ExceptionMessage">
        <template #body="{ data }">
          <div
            class="flex items-center gap-2"
            @click="displayPopMsg($event, data, 'exceptionMessage')"
            v-tooltip.top="'click to view detail'"
          >
            <span class="limited-text"> {{ data.exceptionMessage }}</span>
          </div>
        </template>
      </Column>
      <Column field="exceptionStackTrace" header="ExceptionStackTrace">
        <template #body="{ data }">
          <div
            class="flex items-center gap-2"
            @click="displayPopMsg($event, data, 'exceptionStackTrace')"
            v-tooltip.top="'click to view detail'"
          >
            <span class="limited-text"> {{ data.exceptionStackTrace }}</span>
          </div>
        </template>
      </Column>
      <Column
        v-for="(col, index) of selectedColumns"
        :field="col.field"
        :header="col.header"
        :key="col.field + '_' + index"
        style="max-width: 20rem"
      ></Column>
      <Column field="logTime" header="LogTime">
        <template #body="{ data }">
          {{ formateTime(data.logTime) }}
        </template>
      </Column>
      <Column field="userName" header="UserName"></Column>
    </DataTable>

    <Dialog
      v-model:visible="detailVisible"
      modal
      maximizable
      header="Log Detail"
      :style="{ width: '65rem' }"
      :breakpoints="{ '1199px': '75vw', '575px': '90vw' }"
    >
      <div>
        <div class="mb-2">
          <label class="font-bold">[Method]: </label>
          <Tag
            :value="selectedLog.method"
            :severity="getSeverity(selectedLog.method)"
          ></Tag>
        </div>
        <div class="mb-2">
          <label class="font-bold">[Host]: </label>
          <span>{{ selectedLog.host }}</span>
        </div>
        <div class="mb-2">
          <label class="font-bold">[Path]: </label>
          <span>{{ selectedLog.path }}</span>
        </div>
        <div class="mb-2">
          <label class="font-bold">[QueryString]: </label>
          <span>{{ selectedLog.queryString }}</span>
        </div>
        <div class="mb-2">
          <label class="font-bold">[IpAddress]: </label>
          <span>{{ selectedLog.ipAddress }}</span>
        </div>
        <div class="mb-2">
          <label class="font-bold">[ConnectionId]: </label>
          <span>{{ selectedLog.connectionId }}</span>
        </div>
        <div class="mb-2">
          <label class="font-bold">[TraceId]: </label>
          <span>{{ selectedLog.traceId }}</span>
        </div>
        <div class="mb-2">
          <label class="font-bold">[EventId]: </label>
          <span>{{ selectedLog.eventId }}</span>
        </div>
        <div class="mb-2">
          <label class="font-bold">[EventName]: </label>
          <span>{{ selectedLog.eventName }}</span>
        </div>
        <div class="mb-2">
          <label class="font-bold">[LogLevel]: </label>
          <span>{{ selectedLog.logLevel }}</span>
        </div>
        <div class="mb-2">
          <label class="font-bold">[Logger]: </label>
          <span>{{ selectedLog.logger }}</span>
        </div>
        <div class="mb-2">
          <label class="font-bold">[LogTime]: </label>
          <span>{{ formateTime(selectedLog.logTime) }}</span>
        </div>
        <div class="mb-2" v-if="selectedLog.userName">
          <label class="font-bold">[UserName]: </label>
          <span>{{ selectedLog.userName }}</span>
        </div>
        <div class="mb-2">
          <label class="font-bold">[Message]: </label>
          <span>{{ selectedLog.message }}</span>
        </div>
        <div class="mb-2">
          <label class="font-bold">[ExceptionSource]: </label>
          <slan>{{ selectedLog.exceptionSource }}</slan>
        </div>
        <div class="mb-2">
          <label class="font-bold">[ExceptionMessage]: </label>
          <slan>{{ selectedLog.exceptionMessage }}</slan>
        </div>

        <Tabs value="0">
          <TabList>
            <Tab value="0">ExceptionStackTrace</Tab>
            <Tab value="1">RequestBody</Tab>
            <Tab value="2">RequestHeaders</Tab>
          </TabList>
          <TabPanels>
            <TabPanel value="0">
              <p class="m-0">
                {{ selectedLog.exceptionStackTrace }}
              </p>
            </TabPanel>
            <TabPanel value="1">
              <p class="m-0">
                {{ selectedLog.requestBody }}
              </p>
            </TabPanel>
            <TabPanel value="2">
              <p class="m-0">
                {{ selectedLog.requestHeaders }}
              </p>
            </TabPanel>
          </TabPanels>
        </Tabs>
      </div>
    </Dialog>

    <Dialog
      v-model:visible="deleteDialogVisible"
      modal
      header="Confirm"
      :style="{ width: '450px' }"
    >
      <div class="flex items-center gap-4">
        <i class="pi pi-exclamation-triangle !text-3xl" />
        <span v-if="selectedTbl"
          >Are you sure you want to delete the selected logs?</span
        >
      </div>
      <template #footer>
        <Button
          label="No"
          icon="pi pi-times"
          text
          @click="deleteDialogVisible = false"
        />
        <Button
          label="Yes"
          icon="pi pi-check"
          text
          @click="confirmDeleteSelected"
        />
      </template>
    </Dialog>

    <Popover ref="msgPop">
      <div v-if="selectedLog" class="rounded flex flex-col">
        <div class="flex justify-center rounded">
          <div class="relative mx-auto">
            <span>{{ dynamicPopMsg }}</span>
          </div>
        </div>
      </div>
    </Popover>
  </div>
</template>

<style lang="css" scoped>
.limited-text {
  display: block;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  max-width: 20ch;
}
</style>
