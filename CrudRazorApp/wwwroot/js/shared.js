// Toast Notification System
const ToastManager = {
    container: null,

    init() {
        if (!this.container) {
            this.container = document.createElement('div');
            this.container.className = 'toast-container';
            document.body.appendChild(this.container);
        }
    },

    show(message, type = 'success', title = null) {
        this.init();

        const toast = document.createElement('div');
        toast.className = `toast ${type}`;

        const icons = {
            success: '<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M22 11.08V12a10 10 0 1 1-5.93-9.14"></path><polyline points="22 4 12 14.01 9 11.01"></polyline></svg>',
            error: '<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><circle cx="12" cy="12" r="10"></circle><line x1="15" y1="9" x2="9" y2="15"></line><line x1="9" y1="9" x2="15" y2="15"></line></svg>',
            warning: '<svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M10.29 3.86L1.82 18a2 2 0 0 0 1.71 3h16.94a2 2 0 0 0 1.71-3L13.71 3.86a2 2 0 0 0-3.42 0z"></path><line x1="12" y1="9" x2="12" y2="13"></line><line x1="12" y1="17" x2="12.01" y2="17"></line></svg>'
        };

        const titles = {
            success: title || 'Éxito',
            error: title || 'Error',
            warning: title || 'Advertencia'
        };

        toast.innerHTML = `
            <div class="toast-icon">${icons[type]}</div>
            <div class="toast-content">
                <div class="toast-title">${titles[type]}</div>
                <div class="toast-message">${message}</div>
            </div>
            <button class="toast-close" onclick="this.parentElement.remove()">×</button>
        `;

        this.container.appendChild(toast);

        setTimeout(() => {
            toast.style.animation = 'slideInRight 0.3s ease reverse';
            setTimeout(() => toast.remove(), 300);
        }, 5000);
    },

    success(message, title) {
        this.show(message, 'success', title);
    },

    error(message, title) {
        this.show(message, 'error', title);
    },

    warning(message, title) {
        this.show(message, 'warning', title);
    }
};

// PDF Download Modal
class PdfModal {
    constructor(formId, tableId) {
        this.form = document.getElementById(formId);
        this.table = document.getElementById(tableId);
        this.modal = null;
        this.selectedOption = 'selected';
        this.init();
    }

    init() {
        this.createModal();
        this.attachEvents();
    }

    createModal() {
        this.modal = document.createElement('div');
        this.modal.className = 'modal-overlay';
        this.modal.id = 'pdfModal';
        this.modal.innerHTML = `
            <div class="modal-content">
                <div class="modal-header">
                    <h3>Descargar Reporte PDF</h3>
                    <button class="modal-close" onclick="document.getElementById('pdfModal').classList.remove('active')">×</button>
                </div>
                <div class="modal-body">
                    <label class="modal-option">
                        <input type="radio" name="pdfOption" value="selected" checked>
                        <div class="modal-option-content">
                            <div class="modal-option-title">Descargar seleccionados</div>
                            <div class="modal-option-description">Descarga solo los registros que has marcado (<span id="selectedCount">0</span> seleccionados)</div>
                        </div>
                    </label>
                    <label class="modal-option">
                        <input type="radio" name="pdfOption" value="all">
                        <div class="modal-option-content">
                            <div class="modal-option-title">Descargar todos</div>
                            <div class="modal-option-description">Descarga todos los registros de la tabla (<span id="totalCount">0</span> registros)</div>
                        </div>
                    </label>
                </div>
                <div class="modal-actions">
                    <button type="button" class="btn-secondary" onclick="document.getElementById('pdfModal').classList.remove('active')">Cancelar</button>
                    <button type="button" class="btn-primary" id="confirmDownload">
                        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                            <path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"></path>
                            <polyline points="7 10 12 15 17 10"></polyline>
                            <line x1="12" y1="15" x2="12" y2="3"></line>
                        </svg>
                        Descargar PDF
                    </button>
                </div>
            </div>
        `;
        document.body.appendChild(this.modal);
    }

    attachEvents() {
        // Cerrar modal al hacer clic fuera
        this.modal.addEventListener('click', (e) => {
            if (e.target === this.modal) {
                this.modal.classList.remove('active');
            }
        });

        // Cambiar opción seleccionada
        const radioButtons = this.modal.querySelectorAll('input[name="pdfOption"]');
        radioButtons.forEach(radio => {
            radio.addEventListener('change', (e) => {
                this.selectedOption = e.target.value;
            });
        });

        // Confirmar descarga
        document.getElementById('confirmDownload').addEventListener('click', () => {
            this.download();
        });
    }

    show() {
        const selectedCount = document.querySelectorAll('.row-checkbox:checked').length;
        const totalCount = this.table.querySelectorAll('tbody tr').length;

        document.getElementById('selectedCount').textContent = selectedCount;
        document.getElementById('totalCount').textContent = totalCount;

        this.modal.classList.add('active');
    }

    download() {
        if (this.selectedOption === 'all') {
            // Seleccionar todos los checkboxes
            const allCheckboxes = document.querySelectorAll('.row-checkbox');
            allCheckboxes.forEach(cb => cb.checked = true);
        }

        const selectedCount = document.querySelectorAll('.row-checkbox:checked').length;

        if (selectedCount === 0) {
            ToastManager.warning('No hay registros seleccionados para descargar');
            return;
        }

        // Enviar formulario
        this.form.submit();
        this.modal.classList.remove('active');

        ToastManager.success(`Descargando reporte con ${selectedCount} registro(s)...`, 'Descarga iniciada');
    }
}

// Enhanced Search
class EnhancedSearch {
    constructor(inputId, tableId, emptyStateId) {
        this.input = document.getElementById(inputId);
        this.table = document.getElementById(tableId);
        this.emptyState = document.getElementById(emptyStateId);
        this.clearBtn = null;
        this.init();
    }

    init() {
        this.createClearButton();
        this.attachEvents();
    }

    createClearButton() {
        const searchBox = this.input.parentElement;
        this.clearBtn = document.createElement('button');
        this.clearBtn.className = 'search-clear';
        this.clearBtn.type = 'button';
        this.clearBtn.innerHTML = '<svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><line x1="18" y1="6" x2="6" y2="18"></line><line x1="6" y1="6" x2="18" y2="18"></line></svg>';
        searchBox.appendChild(this.clearBtn);
    }

    attachEvents() {
        this.input.addEventListener('input', () => this.search());
        this.clearBtn.addEventListener('click', () => this.clear());
    }

    search() {
        const searchValue = this.input.value.toLowerCase().trim();
        const rows = this.table.querySelectorAll('tbody tr');
        let visibleCount = 0;

        // Toggle clear button
        if (searchValue) {
            this.clearBtn.classList.add('active');
        } else {
            this.clearBtn.classList.remove('active');
        }

        rows.forEach(row => {
            const text = row.textContent.toLowerCase();
            if (text.includes(searchValue)) {
                row.style.display = '';
                visibleCount++;
            } else {
                row.style.display = 'none';
                // Desmarcar checkbox si está oculto
                const checkbox = row.querySelector('.row-checkbox');
                if (checkbox) checkbox.checked = false;
            }
        });

        // Toggle empty state
        if (visibleCount === 0 && searchValue) {
            this.table.style.display = 'none';
            this.emptyState.style.display = 'flex';
        } else {
            this.table.style.display = 'table';
            this.emptyState.style.display = 'none';
        }

        // Update select all checkbox
        const selectAll = document.getElementById('selectAll');
        if (selectAll) selectAll.checked = false;

        // Trigger update if exists
        if (typeof updateDownloadButton === 'function') {
            updateDownloadButton();
        }
    }

    clear() {
        this.input.value = '';
        this.clearBtn.classList.remove('active');
        this.search();
        this.input.focus();
    }
}

// Checkbox Manager
class CheckboxManager {
    constructor(selectAllId) {
        this.selectAll = document.getElementById(selectAllId);
        this.checkboxes = document.querySelectorAll('.row-checkbox');
        this.downloadBtn = document.getElementById('btnDescargarPdf');
        this.countSpan = document.getElementById('countSelected');
        this.init();
    }

    init() {
        this.attachEvents();
    }

    attachEvents() {
        if (this.selectAll) {
            this.selectAll.addEventListener('change', () => {
                const visibleCheckboxes = Array.from(this.checkboxes).filter(cb => {
                    return cb.closest('tr').style.display !== 'none';
                });
                visibleCheckboxes.forEach(cb => cb.checked = this.selectAll.checked);
                this.updateDownloadButton();
            });
        }

        this.checkboxes.forEach(cb => {
            cb.addEventListener('change', () => this.updateDownloadButton());
        });
    }

    updateDownloadButton() {
        const checkedCount = document.querySelectorAll('.row-checkbox:checked').length;
        this.countSpan.textContent = checkedCount;
        this.downloadBtn.style.display = checkedCount > 0 ? 'inline-flex' : 'none';
    }
}

// Global function for compatibility
function updateDownloadButton() {
    const checkedCount = document.querySelectorAll('.row-checkbox:checked').length;
    const countSpan = document.getElementById('countSelected');
    const downloadBtn = document.getElementById('btnDescargarPdf');

    if (countSpan) countSpan.textContent = checkedCount;
    if (downloadBtn) downloadBtn.style.display = checkedCount > 0 ? 'inline-flex' : 'none';
}

// Check for URL parameters for success/error messages
document.addEventListener('DOMContentLoaded', () => {
    const urlParams = new URLSearchParams(window.location.search);

    if (urlParams.has('success')) {
        const message = urlParams.get('message') || 'Operación completada exitosamente';
        ToastManager.success(message);

        // Limpiar URL
        window.history.replaceState({}, document.title, window.location.pathname);
    }

    if (urlParams.has('error')) {
        const message = urlParams.get('message') || 'Ha ocurrido un error';
        ToastManager.error(message);

        // Limpiar URL
        window.history.replaceState({}, document.title, window.location.pathname);
    }
});