<script setup>
import { nextTick, onBeforeMount, ref } from "vue";
import { useToast } from "primevue/usetoast";
import { LogService } from "@/service/LogService";

const toast = useToast();
const dt = ref();
const tbl = ref();
const selectedTbl = ref();
const loading = ref();
const deleteDialogVisible = ref(false);
const detailVisible = ref(false);
const msgPop = ref();
const dynamicPopMsg = ref("");
const selectedLog = ref();
const selectedColumn = ref();
const pageIndex = ref(1);
const pageSize = ref(20);
const totalRecords = ref(0);

function getSeverity(level) {
  switch (level) {
    case "Information":
      return "info";
    case "Warning":
      return "warn";
    case "Error":
      return "danger";
  }
}
function formateTime(value) {
  return value.toLocaleString("zh-CN");
}
const confirmDeleteSelected = () => {
  var ids = selectedTbl.value.map((x) => x.id);
  LogService.deleteApplicationLogs(ids)
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
        summary: "Failed",
        detail: error.message + "\r\n" + (error.response?.data?.message ?? ""),
        life: 6000,
      });
    });
};
const showDeleteDialog = () => {
  deleteDialogVisible.value = true;
};
const exportCSV = () => {
  dt.value.exportCSV();
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
const onPageChange = (event) => {
  pageIndex.value = event.page + 1;
  pageSize.value = event.rows;
  getAppLogs();
};
const getAppLogs = () => {
  loading.value = true;
  LogService.getApplicationLogs(pageIndex.value, pageSize.value)
    .then((resp) => {
      tbl.value = resp.data;
      totalRecords.value = resp.paging.totalCount;
      loading.value = false;
      tbl.value.forEach((v) => (v.logTime = new Date(v.logTime)));
    })
    .catch((error) => {
      loading.value = false;
      console.log(error);
    });
};
const viewLog = (data) => {
  selectedLog.value = data;
  detailVisible.value = true;
};
onBeforeMount(() => {
  getAppLogs();
});
</script>

<template>
  <div class="card">
    <div class="font-semibold text-xl mb-4">Application Logs</div>
    <DataTable
      ref="dt"
      v-model:selection="selectedTbl"
      :value="tbl"
      :loading="loading"
      :paginator="true"
      :rowsPerPageOptions="[10, 20, 50, 100, 200, 500]"
      :rows="pageSize"
      :totalRecords="totalRecords"
      :rowHover="true"
      :lazy="true"
      currentPageReportTemplate="Showing {first} to {last} of {totalRecords} logs"
      paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
      dataKey="id"
      resizableColumns
      columnResizeMode="fit"
      showGridlines
      scrollable
      scrollHeight="580px"
      tableStyle="min-width: 50rem"
      @page="onPageChange($event)"
    >
      <template #header>
        <div class="flex justify-between">
          <span class="flex justify-between gap-4">
            <Button
              label="Refresh"
              icon="pi pi-refresh"
              @click="getAppLogs"
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
          <span class="flex justify-between gap-4"> </span>
        </div>
      </template>
      <template #empty> No data found. </template>
      <template #loading> Loading application logs. Please wait. </template>
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
      <Column
        field="eventName"
        header="EventName"
        style="max-width: 15rem"
      ></Column>
      <Column field="logLevel" header="LogLevel" style="min-width: 10rem">
        <template #body="{ data }">
          <Tag :value="data.logLevel" :severity="getSeverity(data.logLevel)" />
        </template>
      </Column>
      <Column field="logger" header="Logger"></Column>
      <Column field="message" header="Message" style="max-width: 25rem">
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
      <Column field="logTime" header="LogTime" dataType="date">
        <template #body="{ data }">
          {{ formateTime(data.logTime) }}
        </template>
      </Column>
      <Column field="userName" header="UserName"></Column>
    </DataTable>

    <Dialog
      v-model:visible="deleteDialogVisible"
      :style="{ width: '450px' }"
      header="Confirm"
      :modal="true"
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
          <label class="font-bold">[Message]: </label>
          <span>{{ selectedLog.message }}</span>
        </div>
        <div class="mb-2">
          <label class="font-bold">[LogTime]: </label>
          <span>{{ formateTime(selectedLog.logTime) }}</span>
        </div>
        <div class="mb-2" v-if="selectedLog.userName">
          <label class="font-bold">[UserName]: </label>
          <span>{{ selectedLog.userName }}</span>
        </div>
      </div>
    </Dialog>
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
.p-datatable .p-datatable-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}
</style>
