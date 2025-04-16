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
const auditMsgPop = ref();
const auditEntity = ref();
const msgPop = ref();
const dynamicPopMsg = ref("");
const selectedLog = ref();
const selectedColumn = ref();
const pageIndex = ref(1);
const pageSize = ref(20);
const totalRecords = ref(0);
const expandedRows = ref();
const expandedRowIds = ref([]);

const expandAll = () => {
  expandedRows.value = tbl.value.reduce(
    (acc, p) => (acc[p.id] = true) && acc,
    {},
  );
  expandedRowIds.value = tbl.value.map((x) => x.id);
};
const collapseAll = () => {
  expandedRowIds.value = [];
  expandedRows.value = null;
};
const onRowExpand = (event) => {
  expandedRowIds.value.push(event.data.id);
};
const onRowCollapse = (event) => {
  expandedRowIds.value = expandedRowIds.value.filter((x) => x != event.data.id);
};
const rowClass = (data) => {
  return [{ "!bg-primary !text-primary-contrast" : expandedRowIds.value.indexOf(data.id) > -1 }];
};
const getState = (state) => {
  switch (state) {
    case 2:
      return "Delete";
    case 3:
      return "Update";
    case 4:
      return "Insert";
    default:
      return null;
  }
};
const formateTime = (value) => {
  return value.toLocaleString("zh-CN");
};
const confirmDeleteSelected = () => {
  var ids = selectedTbl.value.map((x) => x.id);
  LogService.deleteAuditLogs(ids)
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
        summary: "delete audit logs failed",
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
const displayAuditMsg = (event, data) => {
  auditMsgPop.value.hide();
  if (auditEntity.value?.id === data.id) {
    auditEntity.value = null;
  } else {
    auditEntity.value = data;
    nextTick(() => {
      auditMsgPop.value.show(event);
    });
  }
};
const onPageChange = (event) => {
  pageIndex.value = event.page + 1;
  pageSize.value = event.rows;
  getAuditLogs();
};
const getAuditLogs = () => {
  loading.value = true;
  LogService.getAuditLogs(pageIndex.value, pageSize.value)
    .then((resp) => {
      tbl.value = resp.data;
      totalRecords.value = resp.paging.totalCount;
      loading.value = false;
      tbl.value.forEach(
        (v) => (
          (v.startTime = new Date(v.startTime)),
          (v.endTime = new Date(v.endTime))
        ),
      );
    })
    .catch((error) => {
      loading.value = false;
      toast.add({
        severity: "error",
        summary: "loading audit logs failed",
        detail: error.message,
        life: 6000,
      });
    });
};
onBeforeMount(() => {
  getAuditLogs();
});
</script>

<template>
  <div class="card">
    <div class="font-semibold text-xl mb-4">Audit Logs</div>
    <DataTable
      ref="dt"
      v-model:selection="selectedTbl"
      v-model:expandedRows="expandedRows"
      :value="tbl"
      :loading="loading"
      :paginator="true"
      :rowsPerPageOptions="[10, 20, 50, 100, 200, 500]"
      :rows="pageSize"
      :totalRecords="totalRecords"
      :rowHover="true"
      :rowClass="rowClass"
      :lazy="true"
      currentPageReportTemplate="Showing {first} to {last} of {totalRecords} logs"
      paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
      dataKey="id"
      resizableColumns
      columnResizeMode="fit"
      scrollable
      scrollHeight="580px"
      tableStyle="min-width: 50rem"
      @page="onPageChange($event)"
      @rowExpand="onRowExpand"
      @rowCollapse="onRowCollapse"
    >
      <template #header>
        <div class="flex justify-between">
          <span class="flex justify-between gap-4">
            <Button
              label="Refresh"
              icon="pi pi-refresh"
              @click="getAuditLogs"
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
            <Button
              text
              icon="pi pi-plus"
              label="Expand All"
              @click="expandAll"
            />
            <Button
              text
              icon="pi pi-minus"
              label="Collapse All"
              @click="collapseAll"
            />
          </span>
        </div>
      </template>
      <template #empty> No data found. </template>
      <template #loading> Loading audit logs. Please wait. </template>
      <Column selectionMode="multiple" headerStyle="width: 3rem"></Column>
      <Column expander style="width: 5rem" />
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
      <Column field="userName" header="UserName"></Column>
      <Column field="startTime" header="StartTime">
        <template #body="{ data }">
          {{ formateTime(data.startTime) }}
        </template>
      </Column>
      <Column field="endTime" header="EndTime">
        <template #body="{ data }">
          {{ formateTime(data.endTime) }}
        </template>
      </Column>
      <Column field="succeeded" header="Succeeded">
        <template #body="{ data }">
          <Tag
            :value="data.succeeded"
            :severity="data.succeeded == true ? 'success' : 'danger'"
          ></Tag>
        </template>
      </Column>
      <Column field="errorMessage" header="ErrorMessage">
        <template #body="{ data }">
          <div
            class="flex items-center gap-2"
            v-tooltip.top="'click to view detail'"
            @click="displayPopMsg($event, data, 'errorMessage')"
          >
            <span class="limited-text"> {{ data.errorMessage }}</span>
          </div>
        </template>
      </Column>
      <template #expansion="slotProps">
        <div class="p-4">
          <DataTable
            :value="slotProps.data.entities"
            tableStyle="max-width: 100%"
          >
            <Column field="state" header="State">
              <template #body="{ data }">
                {{ getState(data.state) }}
              </template>
            </Column>
            <Column field="auditMessage" header="AuditMessage">
              <template #body="{ data }">
                <div
                  class="flex items-center gap-2"
                  v-tooltip.left="'click to view detail'"
                  @click="displayAuditMsg($event, data)"
                >
                  <span class="limited-text-150">{{ data.auditMessage }}</span>
                </div>
              </template>
            </Column>
          </DataTable>
        </div>
      </template>
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

    <Popover ref="auditMsgPop">
      <div v-if="auditEntity" class="rounded flex flex-col">
        <div class="flex justify-center rounded">
          <div class="relative mx-auto">
            <span>{{ auditEntity.auditMessage }}</span>
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
      <div class="mb-2">
        <label class="font-bold">[UserName]: </label>
        <span>{{ selectedLog.userName }}</span>
      </div>
      <div class="mb-2">
        <label class="font-bold">[StartTime]: </label>
        <span>{{ formateTime(selectedLog.startTime) }}</span>
      </div>
      <div class="mb-2">
        <label class="font-bold">[EndTime]:</label>
        <span>{{ formateTime(selectedLog.endTime) }}</span>
      </div>
      <div class="mb-2">
        <label class="font-bold">[Succeeded]:</label>
        <Tag
          :value="selectedLog.succeeded"
          :severity="selectedLog.succeeded == true ? 'success' : 'danger'"
        ></Tag>
      </div>
      <div class="mb-2" v-if="!selectedLog.succeeded">
        <label class="font-bold">[ErrorMessage]: </label>
        <span>{{ selectedLog.errorMessage }}</span>
      </div>
      <DataTable :value="selectedLog.entities">
        <Column field="state" header="State">
          <template #body="{ data }">
            {{ getState(data.state) }}
          </template>
        </Column>
        <Column field="auditMessage" header="AuditMessage"></Column>
      </DataTable>
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
.limited-text-150 {
  display: block;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  max-width: 150ch;
}
</style>
