export type HealthResponse = {
  status: string;
  message: string;
  data?: {
    service: string;
    databaseConnected: boolean;
    checkedAt: string;
  };
};

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL ?? 'https://localhost:7101';

export async function getHealth(signal?: AbortSignal): Promise<HealthResponse> {
  const response = await fetch(`${apiBaseUrl}/api/v1/health`, { signal });

  if (!response.ok) {
    throw new Error(`API returned ${response.status}`);
  }

  return response.json();
}
