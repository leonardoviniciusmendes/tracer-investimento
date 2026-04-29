import { computed, reactive, ref } from "vue";
import { createManualSignal, fetchManualSignals } from "../services/signalsService";

const defaultForm = () => ({
  ticker: "",
  signalType: "buy",
  confidence: 80,
  note: "",
  capturedAt: "",
});

export function useManualSignalsStore() {
  const manualSignals = ref([]);
  const isLoading = ref(false);
  const isSaving = ref(false);
  const errorMessage = ref("");
  const successMessage = ref("");
  const form = reactive(defaultForm());

  const signalCount = computed(() => manualSignals.value.length);

  const latestSignal = computed(() => manualSignals.value[0] ?? null);

  function resetForm() {
    Object.assign(form, defaultForm());
  }

  function normalizeFormPayload() {
    return {
      ticker: form.ticker.trim().toUpperCase(),
      signalType: form.signalType,
      confidence: Number(form.confidence),
      note: form.note.trim(),
      capturedAt: form.capturedAt ? new Date(form.capturedAt).toISOString() : null,
    };
  }

  async function loadManualSignals() {
    isLoading.value = true;
    errorMessage.value = "";

    try {
      manualSignals.value = await fetchManualSignals();
    } catch (error) {
      manualSignals.value = [];
      errorMessage.value =
        error instanceof Error
          ? error.message
          : "Nao foi possivel carregar os sinais manuais.";
    } finally {
      isLoading.value = false;
    }
  }

  async function submitManualSignal() {
    isSaving.value = true;
    errorMessage.value = "";
    successMessage.value = "";

    try {
      await createManualSignal(normalizeFormPayload());
      successMessage.value = "Sinal manual registrado com sucesso.";
      resetForm();
      await loadManualSignals();
    } catch (error) {
      errorMessage.value =
        error instanceof Error
          ? error.message
          : "Nao foi possivel registrar o sinal manual.";
    } finally {
      isSaving.value = false;
    }
  }

  return {
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
    resetForm,
  };
}
