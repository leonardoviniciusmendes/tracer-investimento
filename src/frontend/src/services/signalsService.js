const apiBaseUrl = import.meta.env.VITE_API_URL ?? "http://localhost:8081";

export async function fetchManualSignals() {
  const response = await fetch(`${apiBaseUrl}/api/signals`);

  if (!response.ok) {
    throw new Error("Falha ao carregar sinais manuais.");
  }

  const data = await response.json();

  return Array.isArray(data) ? data : [];
}

export async function createManualSignal(payload) {
  const response = await fetch(`${apiBaseUrl}/api/signals`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(payload),
  });

  if (response.ok) {
    return response.json();
  }

  let message = "Falha ao registrar sinal manual.";

  try {
    const error = await response.json();
    if (error?.detail) {
      message = error.detail;
    } else if (error?.errors) {
      const firstError = Object.values(error.errors).flat().find(Boolean);
      if (firstError) {
        message = firstError;
      }
    }
  } catch {
    message = "Falha ao registrar sinal manual.";
  }

  throw new Error(message);
}
