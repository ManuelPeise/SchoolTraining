import AxiosClient from './../lib/api/AxiosClient';
import { IStatelessApi } from 'src/lib/interfaces/IStatelessApi';

const createStatelessApi = <TResponse, TRequest>(): IStatelessApi<TResponse, TRequest> => {
  return {
    get: get,
    post: post,
    postFile: postFile,
    downloadFile: downloadFile,
  };
};

const get = async <TResponse, TRequest>(url: string, params?: TRequest): Promise<TResponse> => {
  return new Promise<TResponse>((resolve, reject) => {
    AxiosClient.get<TResponse>(url, { params })
      .then((response: any) => {
        resolve(response.data);
      })
      .catch((error: any) => {
        reject(error);
      });
  });
};

const post = async <TResponse, TRequest>(url: string, data: TRequest): Promise<TResponse> => {
  return new Promise<TResponse>((resolve, reject) => {
    AxiosClient.post<TResponse>(url, data)
      .then((response: any) => {
        resolve(response.data);
      })
      .catch((error: any) => {
        reject(error);
        console.error('StatelessApi POST error:', error);
      });
  });
};

const postFile = async <TResponse>(url: string, file: File): Promise<TResponse> => {
  const formData = new FormData();
  formData.append('file', file);

  return new Promise<TResponse>((resolve, reject) => {
    AxiosClient.post<TResponse>(url, formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    })
      .then((response: any) => {
        resolve(response.data);
      })
      .catch((error: any) => {
        reject(error);
        console.error('StatelessApi POST file error:', error);
      });
  });
};

const downloadFile = async (url: string, downloadFileName?: string): Promise<void> => {
  try {
    // Make a GET request to get the file as a blob
    const response = await AxiosClient.get(url, { responseType: 'blob' });

    // Try to extract filename from Content-Disposition header (handle different casings and filename*)
    let filename = downloadFileName;
    let disposition =
      response.headers?.['content-disposition'] || response.headers?.['Content-Disposition'];
    if (!disposition && typeof response.headers?.get === 'function') {
      disposition =
        response.headers.get('content-disposition') || response.headers.get('Content-Disposition');
    }

    if (disposition) {
      // Prefer filename* (RFC 5987)
      const filenameStarMatch = disposition.match(/filename\*=([^;]+)/i);
      if (filenameStarMatch && filenameStarMatch[1]) {
        // e.g. filename*=UTF-8''FamilyImport_FamilyName_YYYYMMDD.json
        const value = filenameStarMatch[1].trim();
        // Remove encoding if present (e.g. UTF-8'')
        const parts = value.split("''");
        if (parts.length === 2) {
          try {
            filename = decodeURIComponent(parts[1]);
          } catch (e) {
            filename = parts[1];
          }
        } else {
          filename = value.replace(/['"]/g, '');
        }
      } else {
        // Fallback to filename=
        const filenameMatch = disposition.match(/filename=([^;]+)/i);
        if (filenameMatch && filenameMatch[1]) {
          filename = filenameMatch[1].trim().replace(/['"]/g, '');
        }
      }
    }

    // Create a blob URL and trigger download
    const urlBlob = window.URL.createObjectURL(response.data);
    const link = document.createElement('a');
    link.href = urlBlob;
    link.setAttribute('download', filename ?? 'downloaded_file');
    document.body.appendChild(link);
    link.click();
    link.remove();
    window.URL.revokeObjectURL(urlBlob);
  } catch (error) {
    throw error;
  }
};

export const StatelessApi = {
  create: createStatelessApi,
};
