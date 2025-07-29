<template>
  <div class="hierarchical-skill-select">
    <label v-if="label" class="form-label">{{ label }}<span v-if="required" class="text-danger">*</span></label>
    
    <!-- Selected Skills Display -->
    <div v-if="selectedSkills.length > 0" class="selected-skills mb-2">
      <div class="selected-skill-tag" v-for="skill in selectedSkills" :key="skill.id">
        <span class="skill-name">{{ skill.name }}</span>
        <button type="button" @click="removeSkill(skill)" class="remove-btn">
          <i class="ri-close-line"></i>
        </button>
      </div>
    </div>

    <!-- Dropdown Toggle -->
    <div class="dropdown-toggle" @click="toggleDropdown" :class="{ 'active': isOpen }">
      <span class="placeholder">{{ placeholder }}</span>
      <i class="ri-arrow-down-s-line toggle-icon"></i>
    </div>

    <!-- Dropdown Content -->
    <div v-if="isOpen" class="dropdown-content">
      <div class="search-box">
        <i class="ri-search-line"></i>
        <input 
          type="text" 
          v-model="searchTerm" 
          placeholder="Skills suchen..."
          class="search-input"
        />
      </div>
      
      <div class="skills-tree">
        <div 
          v-for="category in filteredCategories" 
          :key="category.id"
          class="skill-category"
        >
          <!-- Category Header -->
          <div 
            class="category-header"
            @click="toggleCategory(category.id)"
            :class="{ 'expanded': expandedCategories.includes(category.id) }"
          >
            <i class="ri-folder-line category-icon"></i>
            <span class="category-name">{{ category.name }}</span>
            <i class="ri-arrow-right-s-line expand-icon"></i>
          </div>
          
          <!-- Category Skills -->
          <div 
            v-if="expandedCategories.includes(category.id)"
            class="category-skills"
          >
            <div 
              v-for="skill in getCategorySkills(category.id)"
              :key="skill.id"
              class="skill-item"
              :class="{ 'selected': isSkillSelected(skill) }"
              @click="toggleSkill(skill)"
            >
              <i class="ri-file-line skill-icon"></i>
              <span class="skill-name">{{ skill.name }}</span>
              <i v-if="isSkillSelected(skill)" class="ri-check-line check-icon"></i>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, watch, onMounted, onUnmounted } from 'vue';

interface Skill {
  id: number;
  name: string;
  parentId?: number;
}

interface Props {
  modelValue: Skill[];
  skills: Skill[];
  label?: string;
  placeholder?: string;
  required?: boolean;
}

const props = withDefaults(defineProps<Props>(), {
  placeholder: 'Skills auswählen...',
  required: false
});

const emit = defineEmits<{
  'update:modelValue': [value: Skill[]];
}>();

const isOpen = ref(false);
const searchTerm = ref('');
const expandedCategories = ref<number[]>([]);
const selectedSkills = ref<Skill[]>(props.modelValue || []);

// Close dropdown when clicking outside
const handleClickOutside = (event: Event) => {
  const target = event.target as HTMLElement;
  if (!target.closest('.hierarchical-skill-select')) {
    isOpen.value = false;
  }
};

onMounted(() => {
  document.addEventListener('click', handleClickOutside);
});

onUnmounted(() => {
  document.removeEventListener('click', handleClickOutside);
});

// Get top-level categories (skills without parent)
const categories = computed(() => {
  return props.skills.filter(skill => !skill.parentId);
});

// Get filtered categories based on search
const filteredCategories = computed(() => {
  if (!searchTerm.value) return categories.value;
  
  const searchLower = searchTerm.value.toLowerCase();
  return categories.value.filter(category => {
    // Check if category name matches
    if (category.name.toLowerCase().includes(searchLower)) return true;
    
    // Check if any child skills match
    const childSkills = getCategorySkills(category.id);
    return childSkills.some(skill => skill.name.toLowerCase().includes(searchLower));
  });
});

// Get skills for a specific category
const getCategorySkills = (categoryId: number) => {
  return props.skills.filter(skill => skill.parentId === categoryId);
};

// Toggle dropdown
const toggleDropdown = () => {
  isOpen.value = !isOpen.value;
};

// Toggle category expansion
const toggleCategory = (categoryId: number) => {
  const index = expandedCategories.value.indexOf(categoryId);
  if (index > -1) {
    expandedCategories.value.splice(index, 1);
  } else {
    expandedCategories.value.push(categoryId);
  }
};

// Toggle skill selection
const toggleSkill = (skill: Skill) => {
  const index = selectedSkills.value.findIndex(s => s.id === skill.id);
  if (index > -1) {
    selectedSkills.value.splice(index, 1);
  } else {
    selectedSkills.value.push(skill);
  }
  emit('update:modelValue', selectedSkills.value);
};

// Remove skill from selection
const removeSkill = (skill: Skill) => {
  const index = selectedSkills.value.findIndex(s => s.id === skill.id);
  if (index > -1) {
    selectedSkills.value.splice(index, 1);
    emit('update:modelValue', selectedSkills.value);
  }
};

// Check if skill is selected
const isSkillSelected = (skill: Skill) => {
  return selectedSkills.value.some(s => s.id === skill.id);
};

// Watch for external changes to modelValue
watch(() => props.modelValue, (newValue) => {
  selectedSkills.value = newValue || [];
}, { deep: true });
</script>

<style scoped>
.hierarchical-skill-select {
  position: relative;
  width: 100%;
}

.form-label {
  display: block;
  margin-bottom: 8px;
  font-weight: 600;
  color: #333;
}

.selected-skills {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
}

.selected-skill-tag {
  display: flex;
  align-items: center;
  background: #e3f2fd;
  border: 1px solid #2196f3;
  border-radius: 16px;
  padding: 4px 12px;
  font-size: 14px;
  color: #1976d2;
}

.remove-btn {
  background: none;
  border: none;
  color: #1976d2;
  margin-left: 8px;
  cursor: pointer;
  padding: 0;
  font-size: 16px;
  display: flex;
  align-items: center;
}

.remove-btn:hover {
  color: #d32f2f;
}

.dropdown-toggle {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 12px 16px;
  border: 1px solid #ddd;
  border-radius: 8px;
  background: white;
  cursor: pointer;
  transition: all 0.2s ease;
}

.dropdown-toggle:hover {
  border-color: #2196f3;
}

.dropdown-toggle.active {
  border-color: #2196f3;
  box-shadow: 0 0 0 3px rgba(33, 150, 243, 0.1);
}

.placeholder {
  color: #666;
}

.toggle-icon {
  transition: transform 0.2s ease;
}

.dropdown-toggle.active .toggle-icon {
  transform: rotate(180deg);
}

.dropdown-content {
  position: absolute;
  top: 100%;
  left: 0;
  right: 0;
  background: white;
  border: 1px solid #ddd;
  border-radius: 8px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  z-index: 1000;
  max-height: 400px;
  overflow: hidden;
}

.search-box {
  position: relative;
  padding: 12px;
  border-bottom: 1px solid #eee;
}

.search-box i {
  position: absolute;
  left: 20px;
  top: 50%;
  transform: translateY(-50%);
  color: #666;
}

.search-input {
  width: 100%;
  padding: 8px 12px 8px 36px;
  border: 1px solid #ddd;
  border-radius: 6px;
  font-size: 14px;
}

.search-input:focus {
  outline: none;
  border-color: #2196f3;
}

.skills-tree {
  max-height: 300px;
  overflow-y: auto;
}

.skill-category {
  border-bottom: 1px solid #f5f5f5;
}

.category-header {
  display: flex;
  align-items: center;
  padding: 12px 16px;
  cursor: pointer;
  transition: background-color 0.2s ease;
}

.category-header:hover {
  background-color: #f8f9fa;
}

.category-header.expanded {
  background-color: #e3f2fd;
}

.category-icon {
  margin-right: 8px;
  color: #ff9800;
  font-size: 18px;
}

.category-name {
  flex: 1;
  font-weight: 600;
  color: #333;
}

.expand-icon {
  transition: transform 0.2s ease;
  color: #666;
}

.category-header.expanded .expand-icon {
  transform: rotate(90deg);
}

.category-skills {
  background-color: #fafafa;
}

.skill-item {
  display: flex;
  align-items: center;
  padding: 8px 16px 8px 32px;
  cursor: pointer;
  transition: background-color 0.2s ease;
}

.skill-item:hover {
  background-color: #f0f0f0;
}

.skill-item.selected {
  background-color: #e8f5e8;
}

.skill-icon {
  margin-right: 8px;
  color: #4caf50;
  font-size: 16px;
}

.skill-name {
  flex: 1;
  color: #333;
}

.check-icon {
  color: #4caf50;
  font-size: 18px;
}

/* Scrollbar Styling */
.skills-tree::-webkit-scrollbar {
  width: 6px;
}

.skills-tree::-webkit-scrollbar-track {
  background: #f1f1f1;
}

.skills-tree::-webkit-scrollbar-thumb {
  background: #c1c1c1;
  border-radius: 3px;
}

.skills-tree::-webkit-scrollbar-thumb:hover {
  background: #a8a8a8;
}
</style> 