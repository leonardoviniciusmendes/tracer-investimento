<script setup>
import { onMounted } from "vue";
import { useManualSignalsStore } from "../stores/useManualSignalsStore";

const {
  manualSignals,
  signalCount,
  latestSignal,
  isLoading,
  isSaving,
  errorMessage,
  successMessage,
  form,
  loadManualSignals,
  submitManualSignal,
} = useManualSignalsStore();

onMounted(loadManualSignals);

function formatCapturedAt(value) {
  if (!value) {
    return "Agora";
  }

  return new Intl.DateTimeFormat("pt-BR", {
    dateStyle: "short",
    timeStyle: "short",
  }).format(new Date(value));
}
</script>

<template>
  <article class="panel signals-panel">
    <div class="panel-header">
      <div>
        <span class="eyebrow">Sinais</span>
        <h2>Ingestao manual</h2>
      </div>
      <p>Registre sinais de compra, venda ou monitoramento sem depender de automacao.</p>
    </div>

    <form class="signals-form" @submit.prevent="submitManualSignal">
      <div class="signals-grid">
        <label class="field">
          <span>Ticker</span>
          <input
            v-model="form.ticker"
            :disabled="isLoading || isSaving"
            type="text"
            maxlength="12"
            placeholder="PETR4"
          />
        </label>

        <label class="field">
          <span>Tipo</span>
          <select v-model="form.signalType" :disabled="isLoading || isSaving">
            <option value="buy">buy</option>
            <option value="sell">sell</option>
            <option value="watch">watch</option>
          </select>
        </label>

        <label class="field">
          <span>Confidencia</span>
          <input
            v-model="form.confidence"
            :disabled="isLoading || isSaving"
            type="number"
            min="0"
            max="100"
            step="1"
          />
        </label>

        <label class="field">
          <span>Data do sinal</span>
          <input
            v-model="form.capturedAt"
            :disabled="isLoading || isSaving"
            type="datetime-local"
          />
        </label>
      </div>

      <label class="field signal-note">
        <span>Observacao</span>
        <textarea
          v-model="form.note"
          :disabled="isLoading || isSaving"
          rows="4"
          maxlength="500"
          placeholder="Motivo do sinal e contexto da leitura."
        />
      </label>

      <div class="signals-actions">
        <button class="ghost-button" type="button" :disabled="isLoading || isSaving" @click="loadManualSignals">
          Atualizar
        </button>
        <button class="primary-button" type="submit" :disabled="isLoading || isSaving">
          {{ isSaving ? "Registrando..." : "Registrar sinal" }}
        </button>
      </div>
    </form>

    <div class="signals-meta">
      <span class="summary-chip">Total: {{ signalCount }}</span>
      <span v-if="latestSignal" class="summary-chip secondary-chip">
        Ultimo: {{ latestSignal.ticker }} - {{ latestSignal.signalType }}
      </span>
    </div>

    <div class="signals-feedback">
      <div v-if="isLoading" class="status-banner loading-banner">
        Carregando sinais...
      </div>
      <div v-else-if="errorMessage" class="status-banner error-banner">
        {{ errorMessage }}
      </div>
      <div v-else-if="successMessage" class="status-banner success-banner">
        {{ successMessage }}
      </div>
    </div>

    <div v-if="manualSignals.length" class="signals-list">
      <article
        v-for="signal in manualSignals"
        :key="signal.id"
        class="signal-card"
      >
        <div class="signal-topline">
          <div>
            <span class="ticker">{{ signal.ticker }}</span>
            <h3>{{ signal.companyName }}</h3>
          </div>
          <span class="signal-type" :data-type="signal.signalType">
            {{ signal.signalType }}
          </span>
        </div>

        <div class="signal-stats">
          <span>Confianca {{ signal.confidence }}%</span>
          <span>{{ formatCapturedAt(signal.capturedAt) }}</span>
        </div>

        <p>{{ signal.note }}</p>
      </article>
    </div>

    <div v-else-if="!isLoading && !errorMessage" class="status-banner empty-banner">
      Nenhum sinal manual registrado ainda.
    </div>
  </article>
</template>
